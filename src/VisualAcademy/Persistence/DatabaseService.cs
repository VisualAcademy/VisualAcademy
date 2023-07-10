using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Features.Buffets;

namespace Persistence
{
    // DbContext와 IDatabaseService를 상속받는 DatabaseService 클래스를 선언합니다.
    public class DatabaseService : DbContext, IDatabaseService
    {
        // 생성자에서 DbContextOptions를 사용하여 기본 클래스의 생성자를 호출하고, 데이터베이스가 생성되도록 합니다.
        public DatabaseService(DbContextOptions<DatabaseService> options) : base(options)
        {
            Database.EnsureCreated(); // 인-메모리 데이터베이스 생성
        }

        // Todo 엔터티에 대한 DbSet를 선언합니다.
        public DbSet<Todo> Todos { get; set; }

        // 변경사항을 저장하는 Save 메서드를 선언합니다.
        public void Save()
        {
            this.SaveChanges();
        }

        // 모델 생성 시 추가적인 구성을 위한 OnModelCreating 메서드를 오버라이드 합니다.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Todo 엔터티에 대한 추가적인 구성을 수행합니다.
            new TodoConfiguration().Configure(modelBuilder.Entity<Todo>());
        }
    }
}
