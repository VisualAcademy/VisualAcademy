using Domain.Entities.Buffets;
using NUnit.Framework;

namespace Domain.UnitTests.Entities.Buffets;

[TestFixture]
public class BrothTests
{
    private Broth _broth = null!;
    private const int Id = 1;
    private const string Name = "콩국물";

    [SetUp]
    public void SetUp()
    {
        _broth = new Broth();
    }

    [Test]
    public void TestSetAndGetId()
    {
        _broth.Id = Id;

        Assert.That(_broth.Id,
            Is.EqualTo(Id));
    }

    [Test]
    public void TestSetAndGetName()
    {
        _broth.Name = Name;

        Assert.That(_broth.Name,
            Is.EqualTo(Name));
    }
}
