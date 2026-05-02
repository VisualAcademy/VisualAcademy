using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Azunt.Web.Models;

public class SitePage
{
    public long Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string RoutePattern { get; set; } = "";

    public string? HttpMethod { get; set; }

    public string? DisplayName { get; set; }

    public string? PageTitle { get; set; }

    public int? PageNumber { get; set; }

    public int SortOrder { get; set; }

    public bool IsPublic { get; set; } = true;

    public bool IsVisibleInDashboard { get; set; } = true;

    public string? RequiredRoles { get; set; }

    public string? RequiredPolicy { get; set; }

    public bool AllowAnonymous { get; set; }

    public bool IsEndpointActive { get; set; } = true;

    public DateTime? LastSyncedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    [NotMapped]
    public string Url
    {
        get
        {
            if (string.IsNullOrWhiteSpace(RoutePattern))
                return "/";

            var route = RoutePattern.Trim();

            return route.StartsWith("/") ? route : "/" + route;
        }
    }
}