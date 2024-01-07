using System.ComponentModel.DataAnnotations;

namespace VisualAcademy.Models;

public class TenantModel
{
    public long Id { get; set; }

    //[Required]
    [Display(Name = "Connection String")]
    public string? ConnectionString { get; set; }

    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; } = null!;

    //[Required]
    [Display(Name = "Authentication Header")]
    public string? AuthenticationHeader { get; set; }

    //[Required]
    [Display(Name = "Account ID")]
    public string? AccountID { get; set; }

    //[Required]
    [Display(Name = "Global Search Connection String")]
    public string? GSConnectionString { get; set; }

    //[Required]
    [Display(Name = "Report Writer URL")]
    public string? ReportWriterURL { get; set; }

    //[Required]
    [Display(Name = "Badge Photo Type")]
    public string? BadgePhotoType { get; set; }

    //[Display(Name = "Portal Name")]
    public string? PortalName { get; set; }
}
