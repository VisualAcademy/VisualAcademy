using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VisualAcademy.Areas.Identity.Models;

namespace VisualAcademy.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //SeedRoles(builder);
    }

    /// <summary>
    /// 기본 역할(Role)들을 생성하는 코드 중 하나 
    /// </summary>
    /// <param name="builder"></param>
    private static void SeedRoles(ModelBuilder builder)
    {
        ////[1] Groups(Roles)
        ////[1][1] ('Administrators', '관리자 그룹', 'Group', '응용 프로그램을 총 관리하는 관리 그룹 계정')
        ////[1][2] ('Everyone', '전체 사용자 그룹', 'Group', '응용 프로그램을 사용하는 모든 사용자 그룹 계정')
        ////[1][3] ('Users', '일반 사용자 그룹', 'Group', '일반 사용자 그룹 계정')
        ////[1][4] ('Guests', '게스트 그룹', 'Group', '게스트 사용자 그룹 계정')

        //builder.Entity<ApplicationRole>().HasData(
        //    //[1][1] Administrators
        //    new ApplicationRole()
        //    {
        //        Name = Dul.Roles.Administrators.ToString(),
        //        NormalizedName = Dul.Roles.Administrators.ToString().ToUpper(),
        //        Description = "응용 프로그램을 총 관리하는 관리 그룹 계정"
        //    },
        //    //[1][2] Everyone
        //    new ApplicationRole()
        //    {
        //        Name = Dul.Roles.Everyone.ToString(),
        //        NormalizedName = Dul.Roles.Everyone.ToString().ToUpper(),
        //        Description = "응용 프로그램을 사용하는 모든 사용자 그룹 계정"
        //    },
        //    //[1][3] Users
        //    new ApplicationRole()
        //    {
        //        Name = Dul.Roles.Users.ToString(),
        //        NormalizedName = Dul.Roles.Users.ToString().ToUpper(),
        //        Description = "일반 사용자 그룹 계정"
        //    },
        //    //[1][4] Guests
        //    new ApplicationRole()
        //    {
        //        Name = Dul.Roles.Guests.ToString(),
        //        NormalizedName = Dul.Roles.Guests.ToString().ToUpper(),
        //        Description = "게스트 사용자 그룹 계정"
        //    }
        //);
    }

    public void SeedInitialData()
    { 
        // Empty
    }
}
