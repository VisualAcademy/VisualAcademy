using Microsoft.AspNetCore.Identity;

namespace VisualAcademy.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// 사용자의 이름
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// 사용자의 성
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// 사용자의 시간대
    /// </summary>
    public string? Timezone { get; set; }

    /// <summary>
    /// 주소
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 성별
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// 테넌트 ID
    /// </summary>
    public long TenantId { get; set; }
}
