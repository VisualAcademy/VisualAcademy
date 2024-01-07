#nullable disable
namespace VisualAcademy.Models;

public class AllowedIPRange
{
    public int Id { get; set; }
    public string StartIPRange { get; set; }
    public string EndIPRange { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; }
    public long TenantId { get; set; }
}
