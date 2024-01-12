using HawasoTribe.Models;
using Microsoft.EntityFrameworkCore;

namespace HawasoTribe.Data;

public class TribeDbContext(DbContextOptions<TribeDbContext> options) : DbContext(options)
{
    public DbSet<State> States { get; set; } = null!;
    public DbSet<Tribe> Tribes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Initial Catalog=HawasoTribe;Integrated Security=True");
    }
}
