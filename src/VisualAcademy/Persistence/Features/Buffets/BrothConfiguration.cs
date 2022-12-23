using Domain.Entities.Buffets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Features.Buffets
{
    public class BrothConfiguration : IEntityTypeConfiguration<Broth>
    {
        public void Configure(EntityTypeBuilder<Broth> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(b => b.Name).IsRequired().HasMaxLength(25);

            builder.HasData(
                new Broth { Id = 1, Name = "콩국물", IsVegan = true },
                new Broth { Id = 2, Name = "멸치국물", IsVegan = false });
        }
    }
}
