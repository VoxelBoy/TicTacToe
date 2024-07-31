public class TicTacToeModel
{
    public Board GameBoard { get; }
    public Player[] Players { get; }
    public Player CurrentPlayer { get; private set; }

    public TicTacToeModel(int boardSize)
    {
        GameBoard = new Board(boardSize, boardSize);
        Players = new Player[2];
    }

    public void InitializePlayers(bool isAIGame, AIPlayer.Difficulty aiDifficulty = AIPlayer.Difficulty.Medium)
    {
        Players[0] = new Player(Symbol.X, "Player 1");
        Players[1] = isAIGame 
            ? new AIPlayer(Symbol.O, "AI", aiDifficulty) 
            : new Player(Symbol.O, "Player 2");
        CurrentPlayer = Players[0];
    }

    public bool MakeMove(int x, int y)
    {
        return GameBoard.PlaceSymbol(x, y, CurrentPlayer.GetSymbol());
    }

    public bool CheckWin()
    {
        int size = GameBoard.Width; // Assuming the board is square

        // Check rows
        for (int row = 0; row < size; row++)
        {
            if (CheckLine(row, 0, 0, 1))
                return true;
        }

        // Check columns
        for (int col = 0; col < size; col++)
        {
            if (CheckLine(0, col, 1, 0))
                return true;
        }

        // Check diagonals
        if (CheckLine(0, 0, 1, 1))
            return true;
        if (CheckLine(0, size - 1, 1, -1))
            return true;

        return false;
    }

    private bool CheckLine(int startRow, int startCol, int rowIncrement, int colIncrement)
    {
        Symbol startSymbol = GameBoard.GetCell(startRow, startCol);
        if (startSymbol == Symbol.None)
            return false;

        int size = GameBoard.Width;
        for (int i = 1; i < size; i++)
        {
            int row = startRow + i * rowIncrement;
            int col = startCol + i * colIncrement;
            if (GameBoard.GetCell(row, col) != startSymbol)
                return false;
        }
        return true;
    }

    public bool IsBoardFull()
    {
        return GameBoard.IsFull();
    }

    public void SwitchTurn()
    {
        CurrentPlayer = CurrentPlayer == Players[0] ? Players[1] : Players[0];
    }
    
    public void ResetBoard()
    {
        GameBoard.Reset();
    }

    public void ResetCurrentPlayerToFirstPlayer()
    {
        CurrentPlayer = Players[0];
    }
}