using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Features.Buffets;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(b => b.Name).IsRequired().HasMaxLength(25);

        builder.HasData(
            new Todo { Id = 1, Name = "콩국물", IsComplete = true },
            new Todo { Id = 2, Name = "멸치국물", IsComplete = false });
    }
}
