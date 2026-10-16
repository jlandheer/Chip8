using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

public class Cpu
{
    public const int RamSize = 4096;
    private readonly IDisplay _display;
    private readonly IKeyboard _keyboard;
    private byte DelayTimer = 0;
    private byte SoundTimer = 0;
    private Random _rnd = new();
    public bool Running = true;
    private readonly Stopwatch _stopwatch = new Stopwatch();

    public Cpu(IDisplay display, IKeyboard keyboard)
    {
        _display = display;
        _keyboard = keyboard;
        _stopwatch.Start();
    }

    public byte[] Ram { get; private set; } = new byte[RamSize];
    public byte[] V { get; private set; } = new byte[16];
    public ushort I { get; private set; }
    public Stack<ushort> Stack { get; private set; } = new();
    public ushort PC { get; private set; }

    public void Load(byte[] program, ushort at = 0x0200)
    {
        Array.Copy(program, 0, Ram, at, program.Length);
        PC = at;
    }

    public void Step()
    {
        if (PC >= RamSize - 1)
        {
            Running = false;
            return;
        }

        if (_stopwatch.ElapsedMilliseconds > 16)
        {
            if (DelayTimer > 0)
                DelayTimer--;
            if (SoundTimer > 0)
                SoundTimer--;

            _stopwatch.Restart();
        }

        var incPC = true;
        var opCode = (ushort)(Ram[PC + 1] + (Ram[PC] << 8));

        switch (opCode)
        {
            case 0x00e0:
                {
                    _display.Clear();
                    break;
                }
            case 0x00ee:
                {
                    var address = Stack.Pop();
                    PC = address;
                    incPC = false;
                    break;
                }
            case >= 0x1000 and <= 0x1fff:
                {
                    PC = (ushort)(opCode & 0x0fff);
                    incPC = false;
                    break;
                }
            case >= 0x2000 and <= 0x2fff:
                {
                    Stack.Push(PC);
                    PC = (ushort)(opCode & 0x0fff);
                    incPC = false;
                    break;
                }
            case >= 0x3000 and <= 0x3fff:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var kk = (byte)(opCode & 0x00ff);
                    if (V[x] == kk)
                        PC += 2;
                    break;
                }
            case >= 0x4000 and <= 0x4fff:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var kk = (byte)(opCode & 0x00ff);
                    if (V[x] != kk)
                        PC += 2;
                    break;
                }
            case >= 0x5000 and <= 0x5fff when (opCode & 0x1) == 0x0:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    if (V[x] == V[y])
                        PC += 2;
                    break;
                }
            case >= 0x6000 and <= 0x6fff:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var kk = (byte)(opCode & 0x00ff);
                    V[x] = kk;
                    break;
                }
            case >= 0x7000 and <= 0x7fff:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var kk = (byte)(opCode & 0x00ff);
                    V[x] += kk;
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x0:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    V[x] = V[y];
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x1:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    V[x] = (byte)(V[x] | V[y]);
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x2:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    V[x] = (byte)(V[x] & V[y]);
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x3:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    V[x] = (byte)(V[x] ^ V[y]);
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x4:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    V[0xF] = (byte)(V[x] + V[y] > 255 ? 1 : 0);
                    V[x] += V[y];
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x5:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    V[0xF] = (byte)(V[x] > V[y] ? 1 : 0);
                    V[x] -= V[y];
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x6:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    V[0xF] = (byte)((V[x] & 0x0001) == 0x0001 ? 1 : 0);
                    V[x] <<= 1;
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x7:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    V[0xF] = (byte)(V[y] > V[x] ? 1 : 0);
                    V[x] = (byte)(V[y] - V[x]);
                    break;
                }
            case >= 0x8000 and <= 0x8fff when (opCode & 0x00f) == 0x0e:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    V[0xF] = (byte)((V[x] & 0b1000_0000_0000_0000) == 0b1000_0000_0000_0000 ? 1 : 0);
                    V[x] <<= 1;
                    break;
                }
            case >= 0x9000 and <= 0x9fff when (opCode & 0x1) == 0x0:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    if (V[x] != V[y])
                        PC += 2;
                    break;
                }
            case >= 0xA000 and <= 0xAfff:
                {
                    var addr = (ushort)(opCode & 0x0fff);
                    I = addr;
                    break;
                }
            case >= 0xB000 and <= 0xBfff:
                {
                    var addr = (ushort)(opCode & 0x0fff);
                    PC = (ushort)(V[0] + addr);
                    incPC = false;
                    break;
                }
            case >= 0xC000 and <= 0xCfff:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var kk = (byte)(opCode & 0x00ff);
                    var rand = _rnd.Next(255);
                    V[x] = (byte)(rand & kk);
                    break;
                }
            case >= 0xD000 and <= 0xDfff:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var y = (byte)((opCode & 0x00f0) >> 4);
                    var height = (byte)(opCode & 0x000f);
                    V[0xf] = (byte)(_display.ShowSprite(Ram[I..(I + height)], V[x], V[y]) ? 1 : 0);
                    break;
                }
            case >= 0xE000 and <= 0xEfff when (opCode & 0xff) == 0x9e:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    if (_keyboard.IsDown(x))
                        PC += 2;
                    break;
                }
            case >= 0xE000 and <= 0xEfff when (opCode & 0xff) == 0xa1:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    if (!_keyboard.IsDown(x))
                        PC += 2;
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x07:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    V[x] = DelayTimer;
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x0a:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var keyCode = _keyboard.WaitForAnyKey();
                    V[x] = keyCode;
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x15:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    DelayTimer = V[x];
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x18:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    SoundTimer = V[x];
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x1e:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    I += V[x];
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x29:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    I = (ushort)(5 * V[x]);
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x33:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    var value = V[x];
                    var hun = value / 100;
                    var dec = (value - 100 * hun) / 10;
                    var ones = value - 100 * hun - 10 * dec;
                    Ram[I] = (byte)hun;
                    Ram[I + 1] = (byte)dec;
                    Ram[I + 2] = (byte)ones;
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x55:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    for (var i = 0; i < x; i++)
                    {
                        Ram[I + i] = V[i];
                    }
                    break;
                }
            case >= 0xF000 and <= 0xFfff when (opCode & 0xff) == 0x65:
                {
                    var x = (byte)((opCode & 0x0f00) >> 8);
                    for (var i = 0; i < x; i++)
                    {
                        V[i] = Ram[I + i];
                    }
                    break;
                }
            default:
                throw new NotImplementedException($"Opcode {opCode:X4} not implemented,");
        }

        if (incPC)
            PC += 2;
    }
}

public interface IDisplay
{
    void Clear();
    bool ShowSprite(byte[] sprite, byte x, byte y);
}

public interface IKeyboard
{
    bool IsDown(byte keyCode);
    byte WaitForAnyKey();
}