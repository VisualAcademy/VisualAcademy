using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using VisualAcademy.Areas.Identity;
using VisualAcademy.Areas.Identity.Models;
using VisualAcademy.Areas.Identity.Services;
using VisualAcademy.Data;
using VisualAcademy.Models.Candidates;

namespace VisualAcademy
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //[!] ConfigureServices... Startup.cs 파일에서 ConfigureServices 메서드 영역: 

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            #region 새로운 DbContext 추가 - CandidateAppDbContext
            // 새로운 DbContext 추가 

            //[a] MVC, RazorPages, Web API에서는 DbContext 사용 가능
            //builder.Services.AddDbContext<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));

            //[b] Blazor Server에서는 DbContextFactory 사용 권장
            builder.Services.AddDbContextFactory<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));
            #endregion

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
            //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;

                    // options.Password.RequireDigit = false; 
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Identity 옵션 설정
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // 암호 설정
                options.Password.RequiredLength = 8; // 암호는 최소 8자로 
                options.Password.RequireDigit = true; // 숫자 반드시 포함
                options.Password.RequireLowercase = true; // 소문자 반드시 포함
                options.Password.RequireNonAlphanumeric = true; // 알파벳 이외의 문자 필요  

                // 잠금 설정
                options.Lockout.MaxFailedAccessAttempts = 5; // 5번 시도 후 잠금
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); // 10분 후 잠금 해제

                // 사용자 설정
                options.User.RequireUniqueEmail = true; // 이메일 중복 방지
            });

            // Google, Microsoft, GitHub Authentication
            //builder.Services.AddAuthentication();

            builder.Services.AddRazorPages(); // Razor Pages
            builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddControllersWithViews(); // MVC

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 종속성 주입 추가 
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            var app = builder.Build();

            //[!] Configure... Startup.cs 파일에서 Configure 메서드 영역: 

            // 개발 환경에서 Update-Database, Seed 데이터 추가
            if (app.Environment.IsDevelopment())
            {
                //await CheckCandidateDbMigrated(app.Services);
                CandidateSeedData(app);
                CandidateDbInitializer.Initialize(app);
            }

            #region SeedInitialData
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var ctx = services.GetRequiredService<ApplicationDbContext>();
                //ctx.Database.Migrate();

                if (app.Environment.IsDevelopment())
                {
                    ctx.SeedInitialData();
                }
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI();

                // 시스템 기본 제공 사용자 및 역할 만들기(처음 애플리케이션 가동할 때)
                await CreateBuiltInUsersAndRoles(app);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"); // MVC 라우팅
            app.MapRazorPages();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }

        #region Create BuiltIn Users and Roles 
        /// <summary>
        /// 내장 사용자 및 그룹(역할) 생성
        /// </summary>
        private static async Task CreateBuiltInUsersAndRoles(WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var serviceProvider = serviceScope.ServiceProvider;

            //[0] DbContext 개체 생성
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated(); // 데이터베이스가 생성되어 있는지 확인 

            // 기본 내장 사용자 및 역할이 하나도 없으면(즉, 처음 데이터베이스 생성이라면)
            if (!dbContext.Users.Any() && !dbContext.Roles.Any())
            {
                string domainName = "visualacademy.com";

                //[1] Groups(Roles)
                var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                //[1][1] ('Administrators', '관리자 그룹', 'Group', '응용 프로그램을 총 관리하는 관리 그룹 계정')
                //[1][2] ('Everyone', '전체 사용자 그룹', 'Group', '응용 프로그램을 사용하는 모든 사용자 그룹 계정')
                //[1][3] ('Users', '일반 사용자 그룹', 'Group', '일반 사용자 그룹 계정')
                //[1][4] ('Guests', '게스트 그룹', 'Group', '게스트 사용자 그룹 계정')
                //string[] roleNames = { Dul.Roles.Administrators.ToString(), Dul.Roles.Everyone.ToString(), Dul.Roles.Users.ToString(), Dul.Roles.Guests.ToString() };
                //foreach (var roleName in roleNames)
                //{
                //    var roleExist = await roleManager.RoleExistsAsync(roleName);
                //    if (!roleExist)
                //    {
                //        await roleManager.CreateAsync(new ApplicationRole { Name = roleName, NormalizedName = roleName.ToUpper(), Description = "" }); // 빌트인 그룹 생성
                //    }
                //}
                //[1][1] Administrators
                var administrators = new ApplicationRole
                {
                    Name = Dul.Roles.Administrators.ToString(),
                    NormalizedName = Dul.Roles.Administrators.ToString().ToUpper(),
                    Description = "응용 프로그램을 총 관리하는 관리 그룹 계정"
                };
                if (!(await roleManager.RoleExistsAsync(administrators.Name)))
                {
                    await roleManager.CreateAsync(administrators);
                }
                //[1][2] Everyone
                var everyone = new ApplicationRole
                {
                    Name = Dul.Roles.Everyone.ToString(),
                    NormalizedName = Dul.Roles.Everyone.ToString().ToUpper(),
                    Description = "응용 프로그램을 사용하는 모든 사용자 그룹 계정"
                };
                if (!(await roleManager.RoleExistsAsync(everyone.Name)))
                {
                    await roleManager.CreateAsync(everyone);
                }
                //[1][3] Users
                var users = new ApplicationRole
                {
                    Name = Dul.Roles.Users.ToString(),
                    NormalizedName = Dul.Roles.Users.ToString().ToUpper(),
                    Description = "일반 사용자 그룹 계정"
                };
                if (!(await roleManager.RoleExistsAsync(users.Name)))
                {
                    await roleManager.CreateAsync(users);
                }
                //[1][4] Guests
                var guests = new ApplicationRole
                {
                    Name = Dul.Roles.Guests.ToString(),
                    NormalizedName = Dul.Roles.Guests.ToString().ToUpper(),
                    Description = "게스트 사용자 그룹 계정"
                };
                if (!(await roleManager.RoleExistsAsync(guests.Name)))
                {
                    await roleManager.CreateAsync(guests);
                }

                //[2] Users
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                //[2][1] Administrator
                // ('Administrator', '관리자', 'User', '응용 프로그램을 총 관리하는 사용자 계정')
                ApplicationUser? administrator = await userManager.FindByEmailAsync($"administrator@{domainName}");
                if (administrator == null)
                {
                    administrator = new ApplicationUser()
                    {
                        UserName = $"administrator@{domainName}",
                        Email = $"administrator@{domainName}",
                        EmailConfirmed = true, // 기본 템플릿에는 이메일 인증 과정을 거치기때문에 EmailConfirmed 속성을 true로 설정해야 함.
                    };
                    await userManager.CreateAsync(administrator, "Pa$$w0rd");
                }

                //[2][2] Guest
                // ('Guest', '게스트 사용자', 'User', '게스트 사용자 계정')
                ApplicationUser? guest = await userManager.FindByEmailAsync($"guest@{domainName}");
                if (guest == null)
                {
                    guest = new ApplicationUser()
                    {
                        UserName = "Guest",
                        Email = $"guest@{domainName}",
                        EmailConfirmed = false,
                    };
                    await userManager.CreateAsync(guest, "Pa$$w0rd");
                }

                //[2][3] Anonymous
                // ('Anonymous', '익명 사용자', 'User', '익명 사용자 계정')
                ApplicationUser? anonymous = await userManager.FindByEmailAsync($"anonymous@{domainName}");
                if (anonymous == null)
                {
                    anonymous = new ApplicationUser()
                    {
                        UserName = "Anonymous",
                        Email = $"anonymous@{domainName}",
                        EmailConfirmed = false,
                    };
                    await userManager.CreateAsync(anonymous, "Pa$$w0rd");
                }

                //[3] UsersInRoles: AspNetUserRoles Table
                await userManager.AddToRoleAsync(administrator, Dul.Roles.Administrators.ToString());
                await userManager.AddToRoleAsync(administrator, Dul.Roles.Users.ToString());
                await userManager.AddToRoleAsync(guest, Dul.Roles.Guests.ToString());
                await userManager.AddToRoleAsync(anonymous, Dul.Roles.Guests.ToString());
            }
        }
        #endregion

        #region CandidateSeedData: Candidates 테이블에 기본 데이터 입력
        // Candidates 테이블에 기본 데이터 입력
        static void CandidateSeedData(WebApplication app)
        {
            // https://docs.microsoft.com/ko-kr/aspnet/core/fundamentals/dependency-injection
            // ?view=aspnetcore-6.0#resolve-a-service-at-app-start-up
            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var candidateDbContext = services.GetRequiredService<CandidateAppDbContext>();

                // Candidates 테이블에 데이터가 없을 때에만 데이터 입력
                if (!candidateDbContext.Candidates.Any())
                {
                    candidateDbContext.Candidates.Add(
                        new Candidate { FirstName = "길동", LastName = "홍", IsEnrollment = false });
                    candidateDbContext.Candidates.Add(
                        new Candidate { FirstName = "두산", LastName = "백", IsEnrollment = false });

                    candidateDbContext.SaveChanges();
                }
            }
        }
        #endregion

        #region CheckCandidateDbMigrated: 데이터베이스 마이그레이션 진행
        // 데이터베이스 마이그레이션 진행
        async Task CheckCandidateDbMigrated(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            using var candidateContext = scope.ServiceProvider.GetService<CandidateAppDbContext>();
            if (candidateContext is not null)
            {
                await candidateContext.Database.MigrateAsync();
            }
        }
        #endregion
    }
}
