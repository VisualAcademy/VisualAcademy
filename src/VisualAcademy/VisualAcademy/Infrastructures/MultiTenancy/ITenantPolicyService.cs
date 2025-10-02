namespace Azunt.Web.Infrastructure.MultiTenancy;

/// <summary>
/// 테넌트 정책 서비스 인터페이스.
/// - 편집 권한 강제 허용 여부
/// - 업로드 체크 우회 여부
/// - (콜백 기반) 현재 사용자 역할로 테넌트 매니저 여부 판별
/// </summary>
public interface ITenantPolicyService
{
    /// <summary>주어진 테넌트가 편집 권한 강제 허용 대상인지.</summary>
    bool IsEditOverrideTenant(string? tenantName);

    /// <summary>주어진 테넌트가 업로드 체크 우회 대상인지.</summary>
    bool IsBypassUploadCheckTenant(string? tenantName);

    /// <summary>
    /// 역할 확인 콜백과 후보 테넌트 목록을 기반으로
    /// 사용자(호출부가 보유)의 테넌트 매니저 여부를 판별합니다.
    /// </summary>
    /// <param name="isInRoleAsync">역할 보유 여부를 확인하는 콜백 (예: role => _userManager.IsInRoleAsync(user, role))</param>
    /// <param name="knownTenantNames">검증 후보 테넌트명 목록</param>
    Task<(bool isManager, string? resolvedTenantName)> IsTenantManagerAsync(
        Func<string, Task<bool>> isInRoleAsync,
        IEnumerable<string> knownTenantNames);
}
