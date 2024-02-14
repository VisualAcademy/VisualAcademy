using System.ComponentModel.DataAnnotations;

namespace VisualAcademy.Models.Tenants;

public class AppointmentTypeModel
{
    [Key]
    public long Id { get; set; }

    [Required]
    [Display(Name = "Appointment Type Name")]
    [StringLength(50)]
    public string AppointmentTypeName { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    //[Required]
    [Display(Name = "Date Created")]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [Display(Name = "Tenant")]
    public long? TenantId { get; set; } = null!;

    public TenantModel? Tenant { get; set; } = null!;
}
