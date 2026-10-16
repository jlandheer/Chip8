//using Microsoft.VisualBasic;

Console.OutputEncoding = System.Text.Encoding.UTF8;
//for (var i = 0; i <= 1000; i++)
//{
//    Console.Write(Strings.ChrW(i));
//    if (i % 50 == 0)
//    { // break every 50 chars
//        Console.WriteLine();
//    }
//}
//Console.ReadKey();


var display = new Display();

var cpu = new Cpu(display, new ConsoleKeyboard());
// Load sprites for digits
cpu.Load([
    0xF0, 0x90, 0x90, 0x90, 0xF0,
    0x20, 0x60, 0x20, 0x20, 0x70,
    0xF0, 0x10, 0xF0, 0x80, 0xF0,
    0xF0, 0x10, 0xF0, 0x10, 0xF0,
    0x90, 0x90, 0xF0, 0x10, 0x10,
    0xF0, 0x80, 0xF0, 0x10, 0xF0,
    0xF0, 0x80, 0xF0, 0x90, 0xF0,
    0xF0, 0x10, 0x20, 0x40, 0x40,
    0xF0, 0x90, 0xF0, 0x90, 0xF0,
    0xF0, 0x90, 0xF0, 0x10, 0xF0,
    0xF0, 0x90, 0xF0, 0x90, 0x90,
    0xE0, 0x90, 0xE0, 0x90, 0xE0,
    0xF0, 0x80, 0x80, 0x80, 0xF0,
    0xE0, 0x90, 0x90, 0x90, 0xE0,
    0xF0, 0x80, 0xF0, 0x80, 0xF0,
    0xF0, 0x80, 0xF0, 0x80, 0x80
    ], 0x0);

//var fileName = "C:\\Users\\msn\\Downloads\\c8games\\PONG";
var fileName = "C:\\Users\\ID060771\\Downloads\\Keypad Test [Hap, 2006].ch8";
var bytes = File.ReadAllBytes(fileName);

bytes = [
    0xf0, 0x0a, // Wait for key press and store in V0
    0xf0, 0x29, // Set I to the location of the sprite for the character in V0
    0xD1, 0x15, // Draw the sprite at (V0, V1) with a height of 5 pixels
    0x12, 0x00  // Jump to the start of the program (infinite loop)
    ];
cpu.Load(bytes);

while (cpu.Running)
{
    Debug();
    Console.ReadKey();
    cpu.Step();
    display.Draw();
    //Thread.Sleep(1);
}

void Debug()
{
    Console.SetCursorPosition(0, 0);
    if (cpu.PC < Cpu.RamSize - 1)
    {
        var opCode = (ushort)(cpu.Ram[cpu.PC + 1] + (cpu.Ram[cpu.PC] << 8));
        Console.WriteLine($"OpCode: {opCode:X}");
    }
    else
    {
        Console.WriteLine($"OpCode: STOPPED");
    }
    Console.WriteLine($"PC: {cpu.PC:X4}      ");
    Console.WriteLine($"I : {cpu.I:X4}");
    Console.WriteLine($"      0   1   2   3   4   5   6   7   8   9   A   B   C   D   E   F");
    Console.Write($"V :");
    for (int i = 0; i < cpu.V.Length; i++)
    {
        Console.Write($"  {cpu.V[i]:X2}");
    }
    Console.WriteLine();

    Console.Write($"Stack :");
    for (int i = 0; i < cpu.Stack.Count; i++)
    {
        Console.Write($" {cpu.Stack.Skip(i).First():X2}");
    }
    Console.WriteLine();
}
