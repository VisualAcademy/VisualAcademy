namespace Domain.Entities;

public class Todo : IEntity
{
    public int Id { get; set; }  // 기본 키
    public string? Name { get; set; }  // 할 일 이름 (nullable)
    public bool IsComplete { get; set; }  // 완료 여부
}
