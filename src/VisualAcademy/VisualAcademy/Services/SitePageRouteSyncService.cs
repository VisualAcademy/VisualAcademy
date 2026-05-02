using Azunt.Web.Data;
using Azunt.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Azunt.Web.Services;

public class SitePageRouteSyncService
{
    private readonly IEnumerable<EndpointDataSource> _sources;
    private readonly ApplicationDbContext _context;

    public SitePageRouteSyncService(
        IEnumerable<EndpointDataSource> sources,
        ApplicationDbContext context)
    {
        _sources = sources;
        _context = context;
    }

    public async Task SyncAsync()
    {
        var endpoints = _sources
            .SelectMany(s => s.Endpoints)
            .OfType<RouteEndpoint>();

        var existing = await _context.SitePages.ToListAsync();

        foreach (var page in existing)
            page.IsEndpointActive = false;

        foreach (var endpoint in endpoints)
        {
            var route = endpoint.RoutePattern.RawText;

            if (string.IsNullOrWhiteSpace(route))
                route = endpoint.RoutePattern.ToString();

            if (string.IsNullOrWhiteSpace(route))
                continue;

            var methods = endpoint.Metadata
                .GetMetadata<IHttpMethodMetadata>()?.HttpMethods
                ?? new List<string> { "GET" };

            var action = endpoint.Metadata
                .GetMetadata<ControllerActionDescriptor>();

            // 루트(/) 보정
            if (action?.ControllerName == "Home" &&
                action.ActionName == "Index")
            {
                route = "/";
            }

            var allowAnonymous =
                endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null;

            foreach (var method in methods)
            {
                var found = existing.FirstOrDefault(x =>
                    x.RoutePattern == route &&
                    x.HttpMethod == method);

                if (found == null)
                {
                    _context.SitePages.Add(new SitePage
                    {
                        RoutePattern = route,
                        HttpMethod = method,
                        DisplayName = action?.DisplayName,
                        AllowAnonymous = allowAnonymous,
                        IsEndpointActive = true
                    });
                }
                else
                {
                    found.IsEndpointActive = true;
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}