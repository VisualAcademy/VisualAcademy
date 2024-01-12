using HawasoTribe.Data;
using HawasoTribe.Models;

using var context = new TribeDbContext();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

Console.WriteLine("새로운 주 추가");
context.Add(new State { Name = "New Jersey" });
context.States.Add(new State { Name = "New York" });
context.SaveChanges();

Console.WriteLine("주 목록");
var states = context.States.ToList();
foreach (var state in states)
{
    Console.WriteLine($"{state.StateId}: {state.Name}");
}

// 첫 번째 주만 읽어와서 이름을 SA로 변경합니다.
Console.WriteLine("주 업데이트");
var firstState = context.States.FirstOrDefault();
if (firstState != null)
{
    firstState.Name = "SA";
    context.SaveChanges();
}

// 첫 번째 주만 삭제
Console.WriteLine("첫 번째 주만 삭제");
var firstState2 = context.States.FirstOrDefault();
if (firstState2 != null)
{
    context.States.Remove(firstState2);
    context.SaveChanges();
}

Console.WriteLine("주 목록");
var states2 = context.States.ToList();
foreach (var state in states2)
{
    Console.WriteLine($"{state.StateId}: {state.Name}");
}
