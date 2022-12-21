using NUnit.Framework;

namespace Domain.Entities;

[TestFixture]
public class TodoTests
{
    private Todo _todo = null!;
    private const int Id = 1;
    private const string Name = "Test";

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
