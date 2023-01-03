namespace Domain.Entities;

public class Todo : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}
