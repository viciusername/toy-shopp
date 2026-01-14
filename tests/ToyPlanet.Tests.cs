
using Xunit;
using ToyPlanet.Core;

public class ToyTests
{
    [Fact]
    public void ToyPrice_ShouldBePositive()
    {
        var toy = new Toy { Price = 100 };
        Assert.True(toy.Price > 0);
    }
}
