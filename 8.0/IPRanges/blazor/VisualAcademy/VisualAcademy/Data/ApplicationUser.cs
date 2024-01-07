using Microsoft.AspNetCore.Identity;

namespace VisualAcademy.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// ������� �̸�
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// ������� ��
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// ������� �ð���
    /// </summary>
    public string? Timezone { get; set; }

    /// <summary>
    /// �ּ�
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// �׳�Ʈ ID
    /// </summary>
    public long TenantId { get; set; }
}
