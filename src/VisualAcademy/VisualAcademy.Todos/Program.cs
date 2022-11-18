using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(
    opt => opt.UseInMemoryDatabase("Todos"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var todos = app.MapGroup("/todos");

todos.MapGet("/",
    async (TodoDb db) =>
    await db.Todos.ToListAsync());

todos.MapPost("/",
    async (Todo todo, TodoDb db) =>
    {
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/todos/{todo.Id}", todo);
    });

todos.MapGet("/{id}",
    async Task<Results<Ok<Todo>, NotFound>> (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? TypedResults.Ok(todo)
            : TypedResults.NotFound());

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
