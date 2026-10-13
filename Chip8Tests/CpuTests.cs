using FluentAssertions;

using Moq;

namespace Chip8Tests;

public class CpuTests
{
    [Fact]
    public void Load_Works()
    {
        var cpu = new Cpu(Mock.Of<IDisplay>(), Mock.Of<IKeyboard>());
        cpu.Load([0x00, 0xe0]);
        cpu.Ram[0x200].Should().Be(0x00);
        cpu.Ram[0x201].Should().Be(0xe0);
        cpu.PC.Should().Be(0x200);
    }

    [Fact]
    public void DisplayClearTest()
    {
        var display = new Mock<IDisplay>();
        var cpu = new Cpu(display.Object, Mock.Of<IKeyboard>());
        cpu.Load([0x00, 0xe0]);
        cpu.Step();
        display.Verify(d => d.Clear(), Times.Once);
    }
}
