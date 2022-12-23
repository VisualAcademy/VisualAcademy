namespace Domain.Entities.Buffets;

/// <summary>
/// 국수
/// </summary>
public class Noodle
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? BrothId { get; set; }

    public Broth? Broth { get; set; }
}
