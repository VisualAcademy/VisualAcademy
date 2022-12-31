namespace Domain.Entities.Buffets;

/// <summary>
/// 국물
/// </summary>
public class Broth : IEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsVegan { get; set; }

    public List<Noodle> Noodles { get; set; } = new();
}
