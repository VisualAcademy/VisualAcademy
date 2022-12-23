using Domain.Entities.Buffets;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BuffetDatabaseService>(opt => opt.UseInMemoryDatabase("Buffets"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/broths", async (BuffetDatabaseService db) => await db.Broths.ToListAsync());

app.MapPost("/broths", async (Broth broth, BuffetDatabaseService db) =>
{
    db.Broths.Add(broth);
    await db.SaveChangesAsync();
    return TypedResults.Ok();
});

app.Run();
