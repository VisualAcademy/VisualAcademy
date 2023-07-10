using Domain.Entities.Buffets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Features.Buffets
{
    // Broth 엔터티에 대한 EF Core 구성을 처리하는 클래스입니다.
    public class BrothConfiguration : IEntityTypeConfiguration<Broth>
    {
        public void Configure(EntityTypeBuilder<Broth> builder)
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
                new Broth
                {
                    Id = 1,
                    Name = "콩국물",
                    IsVegan = true
                },
                new Broth
                {
                    Id = 2,
                    Name = "멸치국물",
                    IsVegan = false
                }
            );
        }
    }
}
