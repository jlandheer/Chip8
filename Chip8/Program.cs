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

bytes = [0xD0, 0x05, 0xF0, 0x0A];
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

    var opCode = (ushort)(cpu.Ram[cpu.PC + 1] + (cpu.Ram[cpu.PC] << 8));
    Console.WriteLine($"OpCode: {opCode:X}");
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
