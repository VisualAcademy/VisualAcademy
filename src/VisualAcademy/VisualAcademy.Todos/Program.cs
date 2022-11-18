using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(
    opt => opt.UseInMemoryDatabase("Todos"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/todos", async (TodoDb db) => 
    await db.Todos.ToListAsync());

app.MapPost("/todos", (Todo todo, TodoDb db) => 
{ 
    db.Todos.Add(todo);
    db.SaveChanges();
    return TypedResults.Ok();
});

app.Run();

class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
