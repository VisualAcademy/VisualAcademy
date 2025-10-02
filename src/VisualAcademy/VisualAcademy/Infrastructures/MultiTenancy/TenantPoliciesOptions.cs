namespace Azunt.Web.Infrastructure.MultiTenancy;

/// <summary>
/// Binds appsettings:TenantPolicies
/// </summary>
public class TenantPoliciesOptions
{
    public List<string> EditOverrideTenants { get; set; } = new();
    public List<string> BypassUploadCheckTenants { get; set; } = new();
    public string ManagerRoleSuffix { get; set; } = "Managers";
    public Dictionary<string, string> CustomManagerRoles { get; set; } = new();
}
