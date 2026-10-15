using System.Text;

public class Display : IDisplay
{
    public const int ScreenWidth = 64;
    public const int ScreenHeight = 32;
    public const int StartWidth = 0;
    public const int StartHeight = 7;
    private bool[,] _screen = new bool[ScreenHeight, ScreenWidth];
    private string _padding = new string(' ', StartWidth);

    public void Clear()
    {
        _screen = new bool[ScreenHeight, ScreenWidth];
    }

    public bool ShowSprite(byte[] sprite, byte x, byte y)
    {
        var changed = false;
        for (int row = 0; row < sprite.Length; row++)
        {
            var b = sprite[row];
            var posY = row + y;
            if (posY >= ScreenHeight)
                return changed;

            for (int col = 0; col < 8; col++)
            {
                var posX = col + x;
                if (posX >= ScreenWidth)
                    posX -= ScreenWidth;

                var nw = b > 127;
                var old = _screen[posY, posX];
                _screen[posY, posX] = nw ^ old;

                if (!nw && old) // If new pixel causes clearing of old then changed = true
                    changed = true;

                b <<= 1; // Shift 1 bit left
            }
        }

        return changed;
    }

    public bool PixelAt(byte x, byte y)
    {
        return _screen[y, x];
    }

    public void Draw()
    {
        Console.SetCursorPosition(0, StartHeight);
        var sb = new StringBuilder();
        for (int row = 0; row < ScreenHeight; row++)
        {
            sb.Append(_padding);
            for (int col = 0; col < ScreenWidth; col++)
            {
                sb.Append(_screen[row, col] ? "*" : " ");
            }
            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }
}