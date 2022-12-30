using Application.Common.Interfaces.Buffets;
using Application.Features.Buffets.Broths.Queries.GetBrothsList;
using Domain.Entities.Buffets;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BuffetDatabaseService>(opt => opt.UseInMemoryDatabase("Buffets"));

builder.Services.AddTransient<IBuffetDatabaseService, BuffetDatabaseService>();
builder.Services.AddTransient<IGetBrothsListQuery, GetBrothsListQuery>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/broths/withdbcontext", async (BuffetDatabaseService db) => await db.Broths.ToListAsync());
app.MapPost("/api/broths/withdbcontext", async (Broth broth, BuffetDatabaseService db) =>
{
    db.Broths.Add(broth);
    await db.SaveChangesAsync();
    return TypedResults.Ok();
});

app.MapGet("/api/broths/withquery", (IGetBrothsListQuery query) => query.Execute());

//TODO: withrepository

app.Run();
