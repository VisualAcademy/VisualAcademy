using System.Collections.Immutable;
using Microsoft.Extensions.Options;

namespace Azunt.Web.Infrastructure.MultiTenancy;

/// <summary>
/// appsettings 기반 테넌트 정책 구현:
/// - 편집 강제 허용 / 업로드 우회 / 매니저 판단(콜백 기반)
/// </summary>
public class TenantPolicyService : ITenantPolicyService
{
    private readonly ImmutableHashSet<string> _editOverride;
    private readonly ImmutableHashSet<string> _bypassUpload;
    private readonly string _managerRoleSuffix;
    private readonly IReadOnlyDictionary<string, string> _customManagerRoles;

    public TenantPolicyService(IOptions<TenantPoliciesOptions> options)
    {
        var o = options.Value;
        _editOverride = (o.EditOverrideTenants ?? new()).ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
        _bypassUpload = (o.BypassUploadCheckTenants ?? new()).ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
        _managerRoleSuffix = string.IsNullOrWhiteSpace(o.ManagerRoleSuffix) ? "Managers" : o.ManagerRoleSuffix;
        _customManagerRoles = o.CustomManagerRoles ?? new Dictionary<string, string>();
    }

    public bool IsEditOverrideTenant(string? tenantName) =>
        !string.IsNullOrWhiteSpace(tenantName) && _editOverride.Contains(tenantName!);

    public bool IsBypassUploadCheckTenant(string? tenantName) =>
        !string.IsNullOrWhiteSpace(tenantName) && _bypassUpload.Contains(tenantName!);

    public async Task<(bool isManager, string? resolvedTenantName)> IsTenantManagerAsync(
        Func<string, Task<bool>> isInRoleAsync,
        IEnumerable<string> knownTenantNames)
    {
        // 1) 커스텀 역할 우선
        foreach (var (tenant, roleName) in _customManagerRoles)
            if (await isInRoleAsync(roleName)) return (true, tenant);

        // 2) 규칙: <Tenant><Suffix>
        foreach (var t in knownTenantNames.Distinct(StringComparer.OrdinalIgnoreCase))
            if (await isInRoleAsync($"{t}{_managerRoleSuffix}")) return (true, t);

        return (false, null);
    }
}
