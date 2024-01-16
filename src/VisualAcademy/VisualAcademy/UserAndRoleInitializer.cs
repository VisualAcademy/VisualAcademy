using Microsoft.AspNetCore.Identity;
using VisualAcademy.Models;
using VisualAcademy.Data;

namespace VisualAcademy
{
    public class UserAndRoleInitializer(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task CreateBuiltInUsersAndRoles()
        {
            using var serviceScope = _serviceProvider.CreateScope();

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
                ApplicationUser? administrator = await userManager.FindByEmailAsync($"administrator@{domainName}");
                if (administrator == null)
                {
                    administrator = new ApplicationUser()
                    {
                        UserName = $"administrator@{domainName}",
                        Email = $"administrator@{domainName}",
                        EmailConfirmed = true,
                        LockoutEnd = null, 
                    };
                    await userManager.CreateAsync(administrator, "Pa$$w0rd");
                }

                //[2][2] Guest
                ApplicationUser? guest = await userManager.FindByEmailAsync($"guest@{domainName}");
                if (guest == null)
                {
                    guest = new ApplicationUser()
                    {
                        UserName = "Guest",
                        Email = $"guest@{domainName}",
                        EmailConfirmed = false,
                        LockoutEnd = DateTime.UtcNow.AddYears(1000),
                    };
                    await userManager.CreateAsync(guest, "Pa$$w0rd");
                }

                //[2][3] Anonymous
                ApplicationUser? anonymous = await userManager.FindByEmailAsync($"anonymous@{domainName}");
                if (anonymous == null)
                {
                    anonymous = new ApplicationUser()
                    {
                        UserName = "Anonymous",
                        Email = $"anonymous@{domainName}",
                        EmailConfirmed = false,
                        LockoutEnd = DateTime.UtcNow.AddYears(1000),
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
    }
}
