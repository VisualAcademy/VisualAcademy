using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualAcademy.Models.Buffets;

/// <summary>
/// 국수
/// </summary>
public class Noodle
{
    public int Id { get; set; }

    [Required]
    [StringLength(25)]
    public string? Name { get; set; }

    [Required]
    public int? BrothId { get; set; }

    [ForeignKey("BrothId")] // Foreign Key
    public Broth? Broth { get; set; }
}
