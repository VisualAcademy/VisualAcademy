using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualAcademy.Models;

// TODO: 테이블 이름 복수형으로...
[Table("Property")]
public partial class Property
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; } = "";

    public bool Active { get; set; } = false;
}
