using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AIPlayer : Player
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    private Difficulty difficulty;

    public AIPlayer(Symbol symbol, string name, Difficulty difficulty) : base(symbol, name)
    {
        this.difficulty = difficulty;
    }

    public (int, int) MakeMove(Board board)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return MakeEasyMove(board);
            case Difficulty.Medium:
                return MakeMediumMove(board);
            case Difficulty.Hard:
                return MakeHardMove(board);
            default:
                throw new ArgumentException("Invalid difficulty level");
        }
    }

    private (int, int) MakeEasyMove(Board board)
    {
        var availableMoves = GetAvailableMoves(board);
        var randomIndex = Random.Range(0, availableMoves.Count);
        return availableMoves[randomIndex];
    }

    private (int, int) MakeMediumMove(Board board)
    {
        // 50% chance of making a smart move, 50% chance of making a random move
        return Random.Range(0,2) == 0 ? MakeHardMove(board) : MakeEasyMove(board);
    }

    private (int, int) MakeHardMove(Board board)
    {
        var boardCopy = board.Copy();
        (int bestScore, int bestX, int bestY) = Minimax(boardCopy, symbol, true);
        return (bestX, bestY);
    }

    private (int score, int x, int y) Minimax(Board board, Symbol currentPlayer, bool isMaximizing)
    {
        List<(int, int)> availableMoves = GetAvailableMoves(board);

        if (CheckWin(board, symbol))
            return (1, -1, -1);
        if (CheckWin(board, GetOppositeSymbol(symbol)))
            return (-1, -1, -1);
        if (availableMoves.Count == 0)
            return (0, -1, -1);

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        int bestX = -1, bestY = -1;

        foreach ((int x, int y) in availableMoves)
        {
            board.PlaceSymbol(x, y, currentPlayer);
            (int score, _, _) = Minimax(board, GetOppositeSymbol(currentPlayer), !isMaximizing);
            board.PlaceSymbol(x, y, Symbol.None); // Undo move

            if (isMaximizing && score > bestScore)
            {
                bestScore = score;
                bestX = x;
                bestY = y;
            }
            else if (!isMaximizing && score < bestScore)
            {
                bestScore = score;
                bestX = x;
                bestY = y;
            }
        }

        return (bestScore, bestX, bestY);
    }

    private List<(int, int)> GetAvailableMoves(Board board)
    {
        List<(int, int)> moves = new List<(int, int)>();
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                if (board.GetCell(x, y) == Symbol.None)
                {
                    moves.Add((x, y));
                }
            }
        }
        return moves;
    }

    private bool CheckWin(Board board, Symbol symbolToCheck)
    {
        // Check rows, columns, and diagonals
        for (int i = 0; i < 3; i++)
        {
            if ((board.GetCell(i, 0) == symbolToCheck &&
                 board.GetCell(i, 1) == symbolToCheck &&
                 board.GetCell(i, 2) == symbolToCheck) ||
                (board.GetCell(0, i) == symbolToCheck &&
                 board.GetCell(1, i) == symbolToCheck &&
                 board.GetCell(2, i) == symbolToCheck))
            {
                return true;
            }
        }

        return (board.GetCell(0, 0) == symbolToCheck &&
                board.GetCell(1, 1) == symbolToCheck &&
                board.GetCell(2, 2) == symbolToCheck) ||
               (board.GetCell(0, 2) == symbolToCheck &&
                board.GetCell(1, 1) == symbolToCheck &&
                board.GetCell(2, 0) == symbolToCheck);
    }

    private Symbol GetOppositeSymbol(Symbol sym)
    {
        return sym == Symbol.X ? Symbol.O : Symbol.X;
    }
}