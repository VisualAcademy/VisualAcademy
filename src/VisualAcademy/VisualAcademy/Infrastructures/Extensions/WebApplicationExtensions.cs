using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Azunt.Web.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapAzuntMinimalApis(this WebApplication app)
    {
        app.MapGet("/api/ping", () => Results.Ok(new { pong = DateTime.UtcNow }));
        return app;
    }
}
