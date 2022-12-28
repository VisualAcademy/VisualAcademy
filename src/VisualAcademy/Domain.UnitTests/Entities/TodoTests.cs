using Domain.Entities;
using NUnit.Framework;

namespace Domain.UnitTests.Entities;

[TestFixture]
public class TodoTests
{
    #region TestGetIsComplete
    [Test]
    public void TestGetIsComplete()
    {
        var sut = new Todo();
        Assert.That(sut.IsComplete, Is.False);
    }
    #endregion
    private Todo _todo = null!;
    private const int Id = 1;
    private const string Name = "Domain Test";

    [SetUp]
    public void SetUp()
    {
        _todo = new Todo();
    }

    [Test]
    public void TestSetAndGetId()
    {
        _todo.Id = Id;
        Assert.That(_todo.Id, Is.EqualTo(Id));
    }

    [Test]
    public void TestSetAndGetName()
    {
        _todo.Name = Name;
        Assert.That(_todo.Name, Is.EqualTo(Name));
    }
}
