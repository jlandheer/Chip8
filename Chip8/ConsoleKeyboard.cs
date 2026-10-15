using System.Windows.Input;

internal class ConsoleKeyboard : IKeyboard
{
    public bool IsDown(byte keyCode)
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            var keyMapping = new Dictionary<ConsoleKey, byte>
            {
                { ConsoleKey.D0, 0x0 },
                { ConsoleKey.D1, 0x1 },
                { ConsoleKey.D2, 0x2 },
                { ConsoleKey.D3, 0x3 },
                { ConsoleKey.D4, 0x4 },
                { ConsoleKey.D5, 0x5 },
                { ConsoleKey.D6, 0x6 },
                { ConsoleKey.D7, 0x7 },
                { ConsoleKey.D8, 0x8 },
                { ConsoleKey.D9, 0x9 },
                { ConsoleKey.A, 0xA },
                { ConsoleKey.B, 0xB },
                { ConsoleKey.C, 0xC },
                { ConsoleKey.D, 0xD },
                { ConsoleKey.E, 0xE },
                { ConsoleKey.F, 0xF }
            };

            if (keyMapping.TryGetValue(key, out byte mappedCode))
            {
                return mappedCode == keyCode;
            }
        }
        return false;
    }

    public byte WaitForAnyKey()
    {
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            var keyMapping = new Dictionary<ConsoleKey, byte>
            {
                { ConsoleKey.D0, 0x0 },
                { ConsoleKey.D1, 0x1 },
                { ConsoleKey.D2, 0x2 },
                { ConsoleKey.D3, 0x3 },
                { ConsoleKey.D4, 0x4 },
                { ConsoleKey.D5, 0x5 },
                { ConsoleKey.D6, 0x6 },
                { ConsoleKey.D7, 0x7 },
                { ConsoleKey.D8, 0x8 },
                { ConsoleKey.D9, 0x9 },
                { ConsoleKey.A, 0xA },
                { ConsoleKey.B, 0xB },
                { ConsoleKey.C, 0xC },
                { ConsoleKey.D, 0xD },
                { ConsoleKey.E, 0xE },
                { ConsoleKey.F, 0xF }
            };

            if (keyMapping.TryGetValue(key, out byte mappedCode))
            {
                return mappedCode;
            }
        }
    }
}