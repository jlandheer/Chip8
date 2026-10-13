using System.Windows.Input;

internal class ConsoleKeyboard : IKeyboard
{
    public bool IsDown(byte keyCode)
    {
        var key = Keyboard.
    }

    public byte WaitForAnyKey()
    {
        return 0x0;
        throw new NotImplementedException();
    }
}