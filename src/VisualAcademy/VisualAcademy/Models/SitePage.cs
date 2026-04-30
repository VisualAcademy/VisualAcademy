public class SitePage
{
    public long Id { get; set; }

    public string RoutePattern { get; set; } = "";

    public string? HttpMethod { get; set; }

    public string? DisplayName { get; set; }

    public string? PageTitle { get; set; }

    public int? PageNumber { get; set; }

    public int SortOrder { get; set; }

    public bool IsPublic { get; set; }

    public bool IsVisibleInDashboard { get; set; }

    public string? RequiredRoles { get; set; }

    public string? RequiredPolicy { get; set; }

    public bool AllowAnonymous { get; set; }

    public bool IsEndpointActive { get; set; }

    public string Url => RoutePattern == "/" ? "/" : "/" + RoutePattern.TrimStart('/');
}