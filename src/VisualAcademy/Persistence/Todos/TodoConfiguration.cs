using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Features.Buffets
{
    // Todo 엔터티에 대한 EF Core 구성을 처리하는 TodoConfiguration 클래스를 선언합니다.
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        // Todo 엔터티를 구성하는 Configure 메서드를 선언합니다.
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            // Id를 기본 키로 설정합니다.
            builder.HasKey(x => x.Id);

            // Name 속성은 필수이며, 최대 길이는 25입니다.
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(25);

            // 초기 데이터를 설정합니다.
            // 데이터베이스가 처음 생성되는 시점에 이 데이터가 삽입됩니다.
            builder.HasData
            (
                new Todo
                {
                    Id = 1,
                    Name = "콩국물",
                    IsComplete = true
                },
                new Todo
                {
                    Id = 2,
                    Name = "멸치국물",
                    IsComplete = false
                }
            );
        }
    }
}
