using System;
using System.IO;

// Structure to represent a player
struct Player
{
    public string Name;
    public char Symbol;
}

// Abstract class for the game board
abstract class Board
{
    protected char[,] grid;

    public Board()
    {
        grid = new char[3, 3];
        InitializeGrid();
    }

    protected void InitializeGrid()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                grid[i, j] = '-';
            }
        }
    }

    public abstract void Display();
    public abstract bool PlaceSymbol(int row, int col, char symbol);
    public abstract bool IsGameOver(out char winner);
}

// Class representing the Tic-Tac-Toe game
class TicTacToeGame : Board
{
    public Player[] Players { get; private set; }
    private int currentPlayerIndex;

    public TicTacToeGame(string player1Name, string player2Name)
    {
        Players = new Player[2];
        Players[0] = new Player { Name = player1Name, Symbol = 'X' };
        Players[1] = new Player { Name = player2Name, Symbol = 'O' };
        currentPlayerIndex = 0;
    }

    public override void Display()
    {
        Console.WriteLine("   0  1  2");
        for (int i = 0; i < 3; i++)
        {
            Console.Write($"{i} ");
            for (int j = 0; j < 3; j++)
            {
                Console.Write($" {grid[i, j]} ");
                if (j < 2) Console.Write("|");
            }
            Console.WriteLine();
            if (i < 2) Console.WriteLine("  -----------");
        }
    }

    public override bool PlaceSymbol(int row, int col, char symbol)
    {
        if (row < 0 || row > 2 || col < 0 || col > 2 || grid[row, col] != '-')
            return false;

        grid[row, col] = symbol;
        return true;
    }

    public override bool IsGameOver(out char winner)
    {
        for (int i = 0; i < 3; i++)
        {
            if (grid[i, 0] != '-' && grid[i, 0] == grid[i, 1] && grid[i, 1] == grid[i, 2])
            {
                winner = grid[i, 0];
                return true;
            }

            if (grid[0, i] != '-' && grid[0, i] == grid[1, i] && grid[1, i] == grid[2, i])
            {
                winner = grid[0, i];
                return true;
            }
        }

        if (grid[0, 0] != '-' && grid[0, 0] == grid[1, 1] && grid[1, 1] == grid[2, 2])
        {
            winner = grid[0, 0];
            return true;
        }

        if (grid[0, 2] != '-' && grid[0, 2] == grid[1, 1] && grid[1, 1] == grid[2, 0])
        {
            winner = grid[0, 2];
            return true;
        }

        winner = '-';
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[i, j] == '-')
                    return false;
            }
        }
        return true;
    }

    public void Play()
    {
        Console.WriteLine("Tic-Tac-Toe Game\n");
        while (true)
        {
            Display();
            Console.WriteLine($"{Players[currentPlayerIndex].Name}'s turn ({Players[currentPlayerIndex].Symbol})");
            Console.Write("Enter row (0-2): ");
            string rowInput = Console.ReadLine();
            int row;
            if (rowInput != null && int.TryParse(rowInput, out row) && row >= 0 && row <= 2)
            {
                Console.Write("Enter column (0-2): ");
                string colInput = Console.ReadLine();
                int col;
                if (colInput != null && int.TryParse(colInput, out col) && col >= 0 && col <= 2)
                {
                    if (PlaceSymbol(row, col, Players[currentPlayerIndex].Symbol))
                    {
                        char winner;
                        if (IsGameOver(out winner))
                        {
                            Display();
                            if (winner != '-')
                                Console.WriteLine($"{winner} wins!");
                            else
                                Console.WriteLine("It's a draw!");
                            break;
                        }
                        currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid column input. Please enter a number between 0 and 2.");
                }
            }
            else
            {
                Console.WriteLine("Invalid row input. Please enter a number between 0 and 2.");
            }

        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter player 1 name: ");
        string player1Name = Console.ReadLine();
        Console.WriteLine("Enter player 2 name: ");
        string player2Name = Console.ReadLine();

        TicTacToeGame game = new TicTacToeGame(player1Name, player2Name);
        game.Play();
    }
}
