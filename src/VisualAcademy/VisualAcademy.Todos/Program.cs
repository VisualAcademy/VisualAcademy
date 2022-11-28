using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(
    opt => opt.UseInMemoryDatabase("Todos"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

var todos = app.MapGroup("/todos").RequireAuthorization();

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

todos.MapPut("/{id}", async Task<IResult> (int id, Todo inputTodo, TodoDb db) => 
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null)
    {
        return TypedResults.NotFound();
    }

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
});

todos.MapDelete("/{id}", async Task<IResult> (int id, TodoDb db) => 
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.Ok(todo); 
    }

    return TypedResults.NotFound(); 
});

todos.MapGet("/complete", async (TodoDb db) => 
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

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
