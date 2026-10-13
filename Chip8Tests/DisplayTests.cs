using FluentAssertions;

namespace Chip8Tests;

public class DisplayTests
{
    [Fact]
    public void T0()
    {
        var display = new Display();
        var sprite = new byte[1] { 0xFF };

        var result = display.ShowSprite(sprite, 0, 0);

        result.Should().BeFalse();
        display.PixelAt(0, 0).Should().BeTrue();
        display.PixelAt(1, 0).Should().BeTrue();
        display.PixelAt(2, 0).Should().BeTrue();
        display.PixelAt(3, 0).Should().BeTrue();
        display.PixelAt(4, 0).Should().BeTrue();
        display.PixelAt(5, 0).Should().BeTrue();
        display.PixelAt(6, 0).Should().BeTrue();
        display.PixelAt(7, 0).Should().BeTrue();
    }

    [Fact]
    public void T1()
    {
        var display = new Display();
        var sprite = new byte[1] { 0xFF };

        var result = display.ShowSprite(sprite, 1, 0);

        result.Should().BeFalse();
        display.PixelAt(0, 0).Should().BeFalse();
        display.PixelAt(1, 0).Should().BeTrue();
        display.PixelAt(2, 0).Should().BeTrue();
        display.PixelAt(3, 0).Should().BeTrue();
        display.PixelAt(4, 0).Should().BeTrue();
        display.PixelAt(5, 0).Should().BeTrue();
        display.PixelAt(6, 0).Should().BeTrue();
        display.PixelAt(7, 0).Should().BeTrue();
        display.PixelAt(8, 0).Should().BeTrue();
    }

    [Fact]
    public void T3()
    {
        var display = new Display();
        var sprite = new byte[1] { 0b1000_0000 };

        var result = display.ShowSprite(sprite, 1, 0);

        result.Should().BeFalse();
        display.PixelAt(0, 0).Should().BeFalse();
        display.PixelAt(1, 0).Should().BeTrue();
        display.PixelAt(2, 0).Should().BeFalse();
        display.PixelAt(3, 0).Should().BeFalse();
        display.PixelAt(4, 0).Should().BeFalse();
        display.PixelAt(5, 0).Should().BeFalse();
        display.PixelAt(6, 0).Should().BeFalse();
        display.PixelAt(7, 0).Should().BeFalse();
        display.PixelAt(8, 0).Should().BeFalse();

        sprite = new byte[1] { 0b0000_0000 };
        var result2 = display.ShowSprite(sprite, 1, 0);

        result2.Should().BeTrue();
        display.PixelAt(0, 0).Should().BeFalse();
        display.PixelAt(1, 0).Should().BeTrue();
        display.PixelAt(2, 0).Should().BeFalse();
        display.PixelAt(3, 0).Should().BeFalse();
        display.PixelAt(4, 0).Should().BeFalse();
        display.PixelAt(5, 0).Should().BeFalse();
        display.PixelAt(6, 0).Should().BeFalse();
        display.PixelAt(7, 0).Should().BeFalse();
        display.PixelAt(8, 0).Should().BeFalse();
    }
}