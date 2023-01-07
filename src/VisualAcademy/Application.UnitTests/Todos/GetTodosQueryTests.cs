using Application.Common.Interfaces;
using Application.Todos.Queries.GetTodos;
using Domain.Entities;
using Moq.AutoMock;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.UnitTests.Todos;

[TestFixture]
public class GetTodosQueryTests
{
    private AutoMocker _mocker = null!;
    private Todo _todo = null!;
    private GetTodosQuery _query = null!;

    private const int Id = 1;
    private const string Name = "Todo 1";
    private const bool IsComplete = false;

    [SetUp]
    public void SetUp()
    {
        _mocker = new AutoMocker();

        _todo = new Todo()
        {
            Id = Id,
            Name = Name,
            IsComplete = IsComplete
        };

        _mocker.GetMock<IDatabaseService>()
            .Setup(x => x.Todos)
            .ReturnsDbSet(new List<Todo> { _todo });

        _query = _mocker.CreateInstance<GetTodosQuery>();
    }

    [Test]
    public void TestExecuteShouldReturnListOfTodos()
    {
        var results = _query.Execute();

        var result = results.Single();

        Assert.That(result.Id, Is.EqualTo(Id));
        Assert.That(result.Name, Is.EqualTo(Name));
        Assert.That(result.IsComplete, Is.EqualTo(IsComplete));
    }
}
