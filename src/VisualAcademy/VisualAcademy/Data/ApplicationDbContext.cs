using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VisualAcademy.Components.Pages.ApplicantsTransfers;
using VisualAcademy.Models.Buffets;
using VisualAcademy.Models.Tenants;

namespace VisualAcademy.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    // Users 속성을 통해 AspNetUsers 테이블에 접근
    // public DbSet<ApplicationUser> Users { get; set; } 이 부분은 필요 없습니다.
    // IdentityDbContext<ApplicationUser>가 이미 Users 속성을 제공합니다.

    public DbSet<Broth> Broths { get; set; }

    public DbSet<Noodle> Noodles { get; set; }

    public DbSet<Garnish> Garnishes { get; set; }

    public DbSet<ApplicantTransfer> ApplicantsTransfers { get; set; }

    /// <summary>
    /// 모델(테이블)이 생성될 때 처음 실행 
    /// </summary>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //SeedRoles(builder);

        builder.Entity<Broth>().HasData(
            new Broth() { Id = 1, Name = "콩국물", IsVegan = true },
            new Broth() { Id = 2, Name = "멸치국물", IsVegan = false });

        builder.Entity<Noodle>().HasData(
            new Noodle { Id = 1, Name = "콩국수", BrothId = 1 },
        new Noodle { Id = 2, Name = "잔치국수", BrothId = 2 });

        builder.Entity<TenantModel>().ToTable("Tenants");
        builder.Entity<AppointmentTypeModel>().ToTable("AppointmentsTypes");

        //// 테넌트 초기 데이터 설정
        //builder.Entity<TenantModel>().HasData(
        //    new TenantModel { Id = 1, Name = "Tenant 1" },
        //    new TenantModel { Id = 2, Name = "Tenant 2" }
        //);
    }

    #region SeedRoles: 기본 역할(Role)들을 생성하는 코드 중 하나 
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
    #endregion

    public void SeedInitialData()
    {
        // Empty
    }

    public DbSet<VisualAcademy.Models.Property> Properties { get; set; } = null!;

    public DbSet<VisualAcademy.Models.TenantModel> Tenants { get; set; }
    public DbSet<AppointmentTypeModel> AppointmentsTypes { get; set; }

    public DbSet<VisualAcademy.Models.Movie> Movie { get; set; } = default!;

    public DbSet<AllowedIPRange> AllowedIPRanges { get; set; }
}
