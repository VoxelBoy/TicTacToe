public class Board
{
    private Symbol[,] cells;
    private int width;
    private int height;

    public Board(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Symbol[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = Symbol.None;
            }
        }
    }

    public bool PlaceSymbol(int x, int y, Symbol symbol)
    {
        if (cells[x, y] == Symbol.None)
        {
            cells[x, y] = symbol;
            return true;
        }
        return false;
    }

    public bool IsFull()
    {
        foreach (var cell in cells)
        {
            if (cell == Symbol.None)
            {
                return false;
            }
        }
        return true;
    }

    public void Reset()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = Symbol.None;
            }
        }
    }

    public Symbol GetCell(int x, int y)
    {
        return cells[x, y];
    }

    public Board Copy()
    {
        var copy = new Board(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                copy.cells[x, y] = cells[x, y];
            }
        }

        return copy;
    }

    public int Width => cells.GetLength(0);
    public int Height => cells.GetLength(1);
}