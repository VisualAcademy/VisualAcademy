using Microsoft.AspNetCore.Identity;

namespace VisualAcademy.Data;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Timezone { get; set; }

    // TODO: 
    /// <summary>
    /// 주소
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 성별
    /// </summary>
    public string? Gender { get; set; }
}
