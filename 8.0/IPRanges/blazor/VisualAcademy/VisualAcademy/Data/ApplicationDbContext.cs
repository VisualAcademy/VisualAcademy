using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VisualAcademy.Models;

namespace VisualAcademy.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        public DbSet<TenantModel> Tenants { get; set; }

        public DbSet<AllowedIPRange> AllowedIPRanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 테넌트 초기 데이터 설정
            modelBuilder.Entity<TenantModel>().HasData(
                new TenantModel { Id = 1, Name = "Tenant 1" },
                new TenantModel { Id = 2, Name = "Tenant 2" }
            );
        }
    }
}
