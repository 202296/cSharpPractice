// System.IO namespace is a standard .NET namespace that provides classes for working with input and output operations, such as reading from and writing to files, streams, and directories.

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
    // 2D array to represent the game grid
    protected char[,] grid;

    // Constructor to initialize the grid
    public Board()
    {
        // Initialize grid as a 3x3 array
        grid = new char[3, 3];
        // Fill the grid with empty cells represented by '-'
        InitializeGrid();
    }

    // Method to fill the grid with empty cells
    protected void InitializeGrid()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Set each cell to '-'
                grid[i, j] = '-';
            }
        }
    }

    // Abstract method to display the game board
    public abstract void Display();

    // Abstract method to place a symbol on the board
    public abstract bool PlaceSymbol(int row, int col, char symbol);

    // Abstract method to check if the game is over
    public abstract bool IsGameOver(out char winner);
}

// Class representing the Tic-Tac-Toe game
// regular class TicTacToeGame inherits from an abstract class Board
class TicTacToeGame : Board
{
    // Array to store information about players
    public Player[] Players { get; private set; }
    // Index of the current player
    private int currentPlayerIndex;

    // Constructor to initialize players and set the initial player
    public TicTacToeGame(string player1Name, string player2Name)
    {
        Players = new Player[2];
        // Assign symbols 'X' and 'O' to players
        Players[0] = new Player { Name = player1Name, Symbol = 'X' };
        Players[1] = new Player { Name = player2Name, Symbol = 'O' };
        // Start with player 1
        currentPlayerIndex = 0;
    }

    // Method to display the game board
    public override void Display()
    {
        Console.WriteLine("   0  1  2");
        for (int i = 0; i < 3; i++)
        {
            Console.Write($"{i} ");
            for (int j = 0; j < 3; j++)
            {
                // Display each cell of the grid
                Console.Write($" {grid[i, j]} ");
                if (j < 2) Console.Write("|"); // Add vertical separator
            }
            Console.WriteLine();
            if (i < 2) Console.WriteLine("  -----------"); // Add horizontal separator
        }
    }

    // Method to place a symbol on the board
    public override bool PlaceSymbol(int row, int col, char symbol)
    {
        // Check if the position is valid and not already occupied
        if (row < 0 || row > 2 || col < 0 || col > 2 || grid[row, col] != '-')
            return false;

        // Place the symbol at the specified position
        grid[row, col] = symbol;
        return true;
    }

    // Method to check if the game is over and determine the winner
    public override bool IsGameOver(out char winner)
    {
        // Check rows and columns for a winning combination
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

        // Check diagonals for a winning combination
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

        // If no winner and no empty cells left, it's a draw
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

    // Method to start the game
    public void Play()
    {
        Console.WriteLine("Tic-Tac-Toe Game\n");
        while (true)
        {
            Display(); // Display the current state of the board
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
                        currentPlayerIndex = (currentPlayerIndex + 1) % 2; // Switch players
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
        // Get names of players from user input
        Console.WriteLine("Enter player 1 name: ");
        string player1Name = Console.ReadLine();
        Console.WriteLine("Enter player 2 name: ");
        string player2Name = Console.ReadLine();

        // Create a new TicTacToeGame instance with the provided player names
        TicTacToeGame game = new TicTacToeGame(player1Name, player2Name);
        // Start the game
        game.Play();
    }
}