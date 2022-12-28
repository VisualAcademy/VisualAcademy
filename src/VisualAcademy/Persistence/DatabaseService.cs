using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Features.Buffets;

namespace Persistence;

public class DatabaseService : DbContext, IDatabaseService
{
    public DatabaseService(DbContextOptions<DatabaseService> options) : base(options)
        => Database.EnsureCreated(); // 인-메모리 데이터베이스 생성

    public DbSet<Todo> Todos { get; set; }

    public void Save() => this.SaveChanges();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new TodoConfiguration().Configure(modelBuilder.Entity<Todo>());
    }
}
