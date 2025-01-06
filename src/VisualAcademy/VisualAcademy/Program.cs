using ArticleApp.Models;
using Microsoft.AspNetCore.Components.Authorization;
using RedPlus.Services;
using VisualAcademy.Areas.Identity;
using VisualAcademy.Areas.Identity.Services;
using VisualAcademy.Components.Pages.ApplicantsTransfers;
using VisualAcademy.Infrastructures;
using VisualAcademy.Models.Candidates;
using VisualAcademy.Repositories.Tenants;
using VisualAcademy.Models.TextTemplates;
using Microsoft.Extensions.Options;
using VisualAcademy.Settings.Translators;

namespace VisualAcademy
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // ASP.NET Core ���� ����, ���� ����, �̵���� ���� ���� ����Ѵ�.
            var builder = WebApplication.CreateBuilder(args);

            //[!] ConfigureServices... Startup.cs ���Ͽ��� ConfigureServices �޼��� ����: 

            // Add services to the container.
            builder.Services.AddRazorPages(); // Razor Pages
            builder.Services.AddControllersWithViews(); // MVC

            // �����̳ʿ� ���񽺸� �߰��Ѵ�.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddQuickGridEntityFrameworkAdapter();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            #region ���ο� DbContext �߰� - CandidateAppDbContext
            // ���ο� DbContext �߰� 

            //[a] MVC, RazorPages, Web API������ DbContext ��� ����
            //builder.Services.AddDbContext<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));

            //[b] Blazor Server������ DbContextFactory ��� ����
            builder.Services.AddDbContextFactory<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));
            #endregion

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
            //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();



            #region AddIdentityCore
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false; // ���� Ȯ���� �䱸���� ����
                    options.SignIn.RequireConfirmedEmail = false; // �̸��� Ȯ���� �䱸���� ����

                    // ��й�ȣ ��å ���� (��: ���� ���� ����)
                    // options.Password.RequireDigit = false; 
                })
                //.AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>() // Identity�� ���� EF Core ����� ����
                .AddSignInManager()
                .AddDefaultTokenProviders(); // ��ū ������ ���� �⺻ ������ ��� 
            #endregion



            // Identity �ɼ� ����
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // ��ȣ ����
                options.Password.RequiredLength = 8; // ��ȣ�� �ּ� 8�ڷ� 
                options.Password.RequireDigit = true; // ���� �ݵ�� ����
                options.Password.RequireLowercase = true; // �ҹ��� �ݵ�� ����
                options.Password.RequireNonAlphanumeric = true; // ���ĺ� �̿��� ���� �ʿ�  

                // ��� ����
                options.Lockout.MaxFailedAccessAttempts = 5; // 5�� �õ� �� ���
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); // 10�� �� ��� ����

                // ����� ����
                options.User.RequireUniqueEmail = true; // �̸��� �ߺ� ����
            });

            // Google, Microsoft, GitHub Authentication
            //builder.Services.AddAuthentication();

            builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            builder.Services.AddSingleton<WeatherForecastService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ���Ӽ� ���� �߰� 
            builder.Services.AddTransient<IEmailSender, Areas.Identity.Services.EmailSender>();

            AddDependencyInjectionContainerForArticles(builder);

            #region ArticleApp
            /// <summary>
            /// �Խ���(Articles) ���� ������(���Ӽ�) ���� ���� �ڵ常 ���� ��Ƽ� ����
            /// </summary>
            void AddDependencyInjectionContainerForArticles(WebApplicationBuilder builder)
            {
                // ArticleAppDbContext.cs Inject: New DbContext Add
                builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ArticleAppDbContext>(options =>
                    options.UseSqlServer(connectionString));

                // IArticleRepository.cs Inject: DI Container�� ����(�������丮) ���
                builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
            }
            #endregion

            #region ASP.NET Core Razor Pages ����
            builder.Services.AddTransient<PortfolioServiceJsonFile>(); // DI Container
            #endregion

            builder.Services.AddTransient<IAppointmentTypeRepository, AppointmentTypeRepository>();

            // �����ͺ��̽� �ʱ�ȭ �� ���̱׷��̼�
            try
            {
                await DatabaseHelper.AddOrUpdateRegistrationDate(connectionString);
            }
            catch (Exception)
            {

            }

            builder.Services.AddScoped<ApplicantUploadService>();

            try
            {
                var schemaEnhancer = new DefaultSchemaEnhancerAddColumns(connectionString);
                schemaEnhancer.EnhanceDefaultDatabase();
            }
            catch (Exception)
            {

            }

            // �ؽ�Ʈ���ø� ����: �⺻ CRUD ������ �ڵ�
            builder.Services.AddDependencyInjectionContainerForTextTemplateApp(connectionString);

            // HttpClient ���
            // HttpClient �ν��Ͻ��� DI(Dependency Injection) �����̳ʿ� ����Ͽ� ���뼺�� ����
            builder.Services.AddHttpClient();

            // Azure Translator ���� ���ε�
            builder.Services.Configure<AzureTranslatorSettings>(builder.Configuration.GetSection("AzureTranslator"));
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AzureTranslatorSettings>>().Value);

            // �� ����
            var app = builder.Build();

            //[!] Configure... Startup.cs ���Ͽ��� Configure �޼��� ����: 

            // ���� ȯ�濡�� Update-Database, Seed ������ �߰�
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
            // HTTP ��û ������������ �����Ѵ�.
            if (app.Environment.IsDevelopment())
            {
                // ���� ȯ�濡���� ���̱׷��̼� ��������Ʈ�� ����Ѵ�.
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI();

                #region �ý��� �⺻ ���� ����� �� ���� �����(ó�� ���ø����̼� ������ ��)
                //[old] �ý��� �⺻ ���� ����� �� ���� �����(ó�� ���ø����̼� ������ ��)
                //await CreateBuiltInUsersAndRoles(app);
                //[new] �ý��� �⺻ ���� ����� �� ���� �����(ó�� ���ø����̼� ������ ��)
                var initializer = new UserAndRoleInitializer(app.Services);
                await initializer.CreateBuiltInUsersAndRoles();
                #endregion            
            }
            else
            {
                // � ȯ�濡���� ���� ó�� �̵����� HSTS �̵��� ����Ѵ�.
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // HTTPS ���𷺼ǰ� ���� ���� �̵��� ����Ѵ�.
            app.UseHttpsRedirection();

            // �̵���� �߰� 
            app.UseStaticFiles(); // ������ HTML, CSS, JavaScript, ... ����
            //app.UseFileServer(); // "Microsoft Learn UseFileServer"

            app.UseRouting();

            // ������ ���� �̵��� ����Ѵ�.
            app.UseAuthorization();

            app.UseAntiforgery();

            // ��Ʈ�ѷ� �� Razor ������ �̵��� ����Ѵ�.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"); // MVC �����

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.MapRazorPages(); // Razor Pages
            app.MapDefaultControllerRoute(); // MVC

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            // �����ͺ��̽� �ʱ�ȭ
            InitializeDatabase(app);

            // �� ����
            app.Run();
        }

        static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                // �׳�Ʈ ������ Ȯ�� �� �ʱ�ȭ
                if (!context.Tenants.Any())
                {
                    context.Tenants.AddRange(
                        new TenantModel { Name = "Tenant 1" },
                        new TenantModel { Name = "Tenant 2" }
                    );
                    context.SaveChanges();
                }
            }
        }

        #region Create BuiltIn Users and Roles 
        /// <summary>
        /// ���� ����� �� �׷�(����) ����
        /// </summary>
        private static async Task CreateBuiltInUsersAndRoles(WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var serviceProvider = serviceScope.ServiceProvider;

            //[0] DbContext ��ü ����
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated(); // �����ͺ��̽��� �����Ǿ� �ִ��� Ȯ�� 

            // �⺻ ���� ����� �� ������ �ϳ��� ������(��, ó�� �����ͺ��̽� �����̶��)
            if (!dbContext.Users.Any() && !dbContext.Roles.Any())
            {
                string domainName = "visualacademy.com";

                //[1] Groups(Roles)
                var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                //[1][1] ('Administrators', '������ �׷�', 'Group', '���� ���α׷��� �� �����ϴ� ���� �׷� ����')
                //[1][2] ('Everyone', '��ü ����� �׷�', 'Group', '���� ���α׷��� ����ϴ� ��� ����� �׷� ����')
                //[1][3] ('Users', '�Ϲ� ����� �׷�', 'Group', '�Ϲ� ����� �׷� ����')
                //[1][4] ('Guests', '�Խ�Ʈ �׷�', 'Group', '�Խ�Ʈ ����� �׷� ����')
                //string[] roleNames = { Dul.Roles.Administrators.ToString(), Dul.Roles.Everyone.ToString(), Dul.Roles.Users.ToString(), Dul.Roles.Guests.ToString() };
                //foreach (var roleName in roleNames)
                //{
                //    var roleExist = await roleManager.RoleExistsAsync(roleName);
                //    if (!roleExist)
                //    {
                //        await roleManager.CreateAsync(new ApplicationRole { Name = roleName, NormalizedName = roleName.ToUpper(), Description = "" }); // ��Ʈ�� �׷� ����
                //    }
                //}
                //[1][1] Administrators
                var administrators = new ApplicationRole
                {
                    Name = Dul.Roles.Administrators.ToString(),
                    NormalizedName = Dul.Roles.Administrators.ToString().ToUpper(),
                    Description = "���� ���α׷��� �� �����ϴ� ���� �׷� ����"
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
                    Description = "���� ���α׷��� ����ϴ� ��� ����� �׷� ����"
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
                    Description = "�Ϲ� ����� �׷� ����"
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
                    Description = "�Խ�Ʈ ����� �׷� ����"
                };
                if (!(await roleManager.RoleExistsAsync(guests.Name)))
                {
                    await roleManager.CreateAsync(guests);
                }

                //[2] Users
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                //[2][1] Administrator
                // ('Administrator', '������', 'User', '���� ���α׷��� �� �����ϴ� ����� ����')
                ApplicationUser? administrator = await userManager.FindByEmailAsync($"administrator@{domainName}");
                if (administrator == null)
                {
                    administrator = new ApplicationUser()
                    {
                        UserName = $"administrator@{domainName}",
                        Email = $"administrator@{domainName}",
                        EmailConfirmed = true, // �⺻ ���ø����� �̸��� ���� ������ ��ġ�⶧���� EmailConfirmed �Ӽ��� true�� �����ؾ� ��.
                    };
                    await userManager.CreateAsync(administrator, "Pa$$w0rd");
                }

                //[2][2] Guest
                // ('Guest', '�Խ�Ʈ �����', 'User', '�Խ�Ʈ ����� ����')
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
                // ('Anonymous', '�͸� �����', 'User', '�͸� ����� ����')
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

        #region CandidateSeedData: Candidates ���̺� �⺻ ������ �Է�
        // Candidates ���̺� �⺻ ������ �Է�
        static void CandidateSeedData(WebApplication app)
        {
            // https://docs.microsoft.com/ko-kr/aspnet/core/fundamentals/dependency-injection
            // ?view=aspnetcore-6.0#resolve-a-service-at-app-start-up
            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var candidateDbContext = services.GetRequiredService<CandidateAppDbContext>();

                // Candidates ���̺� �����Ͱ� ���� ������ ������ �Է�
                if (!candidateDbContext.Candidates.Any())
                {
                    candidateDbContext.Candidates.Add(
                        new Candidate { FirstName = "�浿", LastName = "ȫ", IsEnrollment = false });
                    candidateDbContext.Candidates.Add(
                        new Candidate { FirstName = "�λ�", LastName = "��", IsEnrollment = false });

                    candidateDbContext.SaveChanges();
                }
            }
        }
        #endregion

        #region CheckCandidateDbMigrated: �����ͺ��̽� ���̱׷��̼� ����
        // �����ͺ��̽� ���̱׷��̼� ����
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
