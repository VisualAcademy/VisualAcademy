using Microsoft.EntityFrameworkCore;

public class Idea
{
    public int Id { get; set; }
    public string Title { get; set; } = null!; 
}

public class IdeaDbContext : DbContext
{
    public DbSet<Idea> Ideas { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = @"Server=(localdb)\mssqllocaldb;
            Database=DevLecIdea;Trusted_Connection=True";
        optionsBuilder.UseSqlServer(connectionString);
    }
}

class Program
{
    static void Main()
    {
        using (var db = new IdeaDbContext())
        {
            var idea = new Idea { Title = "Entity Framework Core 학습!" };
            db.Ideas.Add(idea);
            db.SaveChanges();

            foreach (var item in db.Ideas)
            {
                Console.WriteLine(item.Title);
            }
        }
    }
}
