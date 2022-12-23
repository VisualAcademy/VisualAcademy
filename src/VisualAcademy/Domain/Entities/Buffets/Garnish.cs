namespace Domain.Entities.Buffets;

/// <summary>
/// 고명 
/// </summary>
public class Garnish
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? NoodleId { get; set; }

    public Noodle? Noodle { get; set; }
}
