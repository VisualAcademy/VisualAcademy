using Application.Common.Interfaces.Buffets;
using Domain.Entities.Buffets;
using Microsoft.EntityFrameworkCore;
using Persistence.Features.Buffets;

namespace Persistence;

public class BuffetDatabaseService : DbContext, IBuffetDatabaseService
{
    public BuffetDatabaseService(DbContextOptions<BuffetDatabaseService> options) : base(options)
    {
        Database.EnsureCreated(); // 인-메모리 데이터베이스 생성
    }

    public DbSet<Broth> Broths { get; set; }

    public void Save()
    {
        this.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new BrothConfiguration().Configure(modelBuilder.Entity<Broth>()); 
    }
}
