
using MinesweeperClassLibrary;
using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Threading;
using System.Xml.Serialization;

namespace Program
{

    class Program
    {

        // Creates instance of Board class
        static Board gameBoard = new Board(20);

        // Game state bool
        static public bool gameEnd { get; set; }

        static void Main(string[] args)
        {
                // Calls the play game method
                PlayGame();
        }

        static public void PlayGame()
        {
                // Print welcome message
                Console.WriteLine("Welcome to Minesweeper!\n");

                // Ask user what size gameBoard they want to play
                int action1 = ChooseBoardSize();
                gameBoard = new Board(action1);

                // Ask user what difficulty they want to play 1 beginner, 2 intermediate, 3 expert
                int action2 = ChooseDifficulty();

                // Calls the Board.setupLiveNeighbors command
                gameBoard.SetupLiveNeighbors(action2, action1);

                // Calls the Board.calculateLiveNeighbors command
                gameBoard.CalculateLiveNeighbors();

                // Calls the printBoard method to display the grid
                PrintBoardDuringGame(action1);

            // In game loop
            while (!gameEnd)
            {
                // If game end is true check status of true
                // If the user selects a bomb -> fail message + reset game
                // If the user reveals all bombs -> success message + reset game

                int userRow;
                int userCol;
                bool allVisited = false;

            userRowSelection:
                {
                    // Fix error checking
                    // Prompt user to choose row
                    Console.WriteLine("\nEnter a Row number:");
                    try { userRow = int.Parse(Console.ReadLine()); }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("Please choose a number between 0 and " + gameBoard.Size + "!");

                        // try again
                        goto userRowSelection;
                    }
                }

            userColSelection:
                {
                    // Fix error checking
                    // Prompt user to choose col
                    Console.WriteLine("Enter a Column number:");
                    try { userCol = int.Parse(Console.ReadLine()); }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("Please choose a number between 0 and " + gameBoard.Size + "!");

                        // try again
                        goto userColSelection;
                    }
                }

                // Check to see if there is a bomb
                if (gameBoard.Grid[userRow, userCol].Live == true)
                {
                    gameEnd = true;
                    Console.WriteLine("You blew up. Game over!");
                    Console.WriteLine("Here is the game board");
                    PrintBoard(gameBoard.Size);

                restart:
                    {
                        int choice = -1;
                        // Ask user if they want to play again
                        Console.WriteLine("\nDo you want to play again? 0 NO, 1 YES:");
                        try
                        {
                            choice = int.Parse(Console.ReadLine());
                            if (choice < 0 || choice > 1)
                            {
                                Console.WriteLine("Please enter 0 or 1!");
                                goto restart;
                            }
                        }
                        catch (System.FormatException e)
                        {
                            Console.Out.WriteLine("Exception Caught: {0}", e + "\n\nPlease enter number values only!");

                            // Try again
                            goto restart;
                        }
                        switch (choice)
                        {
                            case 0:
                                System.Environment.Exit(0);
                                break;
                            case 1:
                                gameEnd = false;
                                PlayGame();
                                break;
                            default:
                                break;
                        }
                    }
                }

                // Set the user selection Visited to true and print gameBoard
                else
                {                   
                    if (gameBoard.Grid[userRow, userCol].NeighborsLive == 0)
                    {
                        gameBoard.FloodFill(userRow, userCol); // Checks for other empty cells with no live neighbors
                    }
                    gameBoard.Grid[userRow, userCol].Visited = true; // Set current selection Cell Visited to true
                    PrintBoardDuringGame(gameBoard.Size); // Prints board

                    int counter = 0;

                    // Checks to see if there are any additional moves left
                    for (int i = 0; i < gameBoard.Size; i++)
                    {
                        for (int j = 0; j < gameBoard.Size; j++)
                        {
                            if (gameBoard.Grid[i, j].Visited == false && gameBoard.Grid[i, j].Live == false)
                            {
                                allVisited = false;
                                counter++;
                            }
                            if (gameBoard.Grid[i, j].Visited == true && gameBoard.Grid[i, j].Live == false)
                            {
                                counter++;
                            }
                        }
                    }

                    // Checks to see if there are any additional moves left
                    for (int i = 0; i < gameBoard.Size; i++)
                    {
                        for (int j = 0; j < gameBoard.Size; j++)
                        {
                            if (gameBoard.Grid[i, j].Visited == true && gameBoard.Grid[i, j].Live == false)
                            {
                                counter--;
                            }
                        }
                    }

                    // Checks for additional moves logic trigger if 0 then game is won
                    if (counter == 0) { allVisited = true; }

                    // Winning game if allVisited is true
                    if (allVisited == true)
                    {
                        gameEnd = true;
                        Console.WriteLine("\nYou Win!! Well done!");

                        Console.WriteLine("Here is the game board");
                        PrintBoard(gameBoard.Size);

                    restart:
                        {
                            int choice = -1;
                            // Ask user if they want to play again
                            Console.WriteLine("\nDo you want to play again? 0 NO, 1 YES:");
                            try
                            {
                                choice = int.Parse(Console.ReadLine());
                                if (choice < 0 || choice > 1)
                                {
                                    Console.WriteLine("Please enter 0 or 1!");
                                    goto restart;
                                }
                            }
                            catch (System.FormatException e)
                            {
                                Console.Out.WriteLine("Exception Caught: {0}", e + "\n\nPlease enter number values only!");

                                // Try again
                                goto restart;
                            }
                            switch (choice)
                            {
                                case 0:
                                    System.Environment.Exit(0);
                                    break;
                                case 1:
                                    gameEnd = false;
                                    PlayGame();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        // Method to print board and test if all is working
        static public void PrintBoard(int size)
        {
            gameBoard.Size = size;

            // Used for for the board printing style while loops
            int counter = 0;

            // Used for column numbers based on board size
            for (int c = 0; c <= gameBoard.Size; c++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (c == gameBoard.Size) { Console.Write("+"); }
                else if (c < 10) { Console.Write("+ " + c + " "); } // Changes style once double digits are hit
                else { Console.Write("+ " + c + ""); }
                Console.Write(""); 
            }
            Console.WriteLine();

            // Used for the styling of the board and placement of the bombs / tiles
            for (int row = 0; row < gameBoard.Size; row++)
            {

                // For the styling of the board for the first row
                while (counter <= gameBoard.Size)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if (counter == gameBoard.Size) { Console.Write("+"); }
                    else { Console.Write("+---"); }
                    counter++;
                }
                counter = 0;

                Console.WriteLine();

                // Places tiles and shows NeighborsLive count as well as places bombs
                for (int col = 0; col < gameBoard.Size; col++)
                {
                    if (gameBoard.Grid[row, col].Live == false)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("| ");  // Tiles
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(gameBoard.Grid[row, col].NeighborsLive.ToString() + " ");
                    }
                    else if (gameBoard.Grid[row, col].Live == true) // Bombs
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("| ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("* ");
                    }
                }

                // Used for placement of row numbers based on size of board
                if (row == -1) { Console.ForegroundColor = ConsoleColor.White; Console.Write(""); } // Adds the negative space for board corner
                else { Console.ForegroundColor = ConsoleColor.White; Console.Write("| " + row); }
                Console.WriteLine();
            }

            // For the styling of the board for all other rows
            while (counter <= gameBoard.Size)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (counter == gameBoard.Size) { Console.Write("+"); }
                else { Console.Write("+---"); }
                counter++;
            }
            counter = 0;
        }

        // Method for handling the board size
        static public int ChooseBoardSize()
        {
            int choice;

        userStart:
            {
                Console.Out.WriteLine("");

                // Prompts the user to make a selection
                Console.Out.Write("Choose your gameBoard size. Must be less than 20: ");

                try
                {
                    // Reads the user input and loads into choice variable
                    choice = int.Parse(Console.ReadLine());
                    if (choice > 20 || choice < 2)
                    {
                        Console.Out.WriteLine("Please enter a number between 2 and 20!");

                        // Try Again
                        goto userStart;
                    }
                }
                catch (System.FormatException e)
                {
                    Console.Out.WriteLine("Exception Caught: {0}", e + "\n\nPlease enter number values only!");

                    // Try again
                    goto userStart;
                }
                return choice;
            }
        }

        // Method for handeling the difficulty
        static public int ChooseDifficulty()
        {
            int choice = 0;

        userStart:
            {
                Console.Out.WriteLine("");

                // Prompts the user to make a selection
                Console.Out.Write("Choose your difficulty level. 1 Beginner, 2 Intermediate, 3 Expert: ");

                try
                {
                    // Reads the user input and loads into choice variable
                    choice = int.Parse(Console.ReadLine());
                    if (choice > 3 || choice < 1)
                    {
                        Console.Out.WriteLine("Please enter a valid option! 1-3.");

                        // Try Again
                        goto userStart;
                    }
                }
                catch (System.FormatException e)
                {
                    Console.Out.WriteLine("Exception Caught: {0}", e + "\n\nPlease enter number values only!");

                    // Try again
                    goto userStart;
                }
                return choice;
            }
        }

        // Method to print board after each new selection
        static public void PrintBoardDuringGame(int size)
        {
            gameBoard.Size = size;

            // Used for for the board printing style while loops
            int counter = 0;

            // Used for column numbers based on board size
            for (int c = 0; c <= gameBoard.Size; c++)
            {
                if (c == gameBoard.Size) { Console.Write("+"); }
                else if (c < 10) { Console.Write("+ " + c + " "); } // Changes style once double digits are hit
                else { Console.Write("+ " + c + ""); }
                Console.Write("");
            }
            Console.WriteLine();

            // Used for the styling of the board and placement of the bombs / tiles
            for (int row = 0; row < gameBoard.Size; row++)
            {

                // For the styling of the board for the first row
                while (counter <= gameBoard.Size)
                {
                    if (counter == gameBoard.Size) { Console.Write("+"); }
                    else { Console.Write("+---"); }
                    counter++;
                }
                counter = 0;

                Console.WriteLine();

                // Places tiles and shows NeighborsLive count as well as places bombs
                for (int col = 0; col < gameBoard.Size; col++)
                {
                    if (gameBoard.Grid[row, col].Visited == false)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("| "); // Unvisited Tiles
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("? ");
                    }
                    else if (gameBoard.Grid[row, col].Visited == true) // LiveNeighbors once visited
                    {
                        if (gameBoard.Grid[row, col].NeighborsLive == 0) 
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("|   "); 
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("| ");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(gameBoard.Grid[row, col].NeighborsLive.ToString() + " ");
                        }
                    }
                }

                // Used for placement of row numbers based on size of board
                if (row == -1) { Console.Write(""); } // Adds the negative space for board corner
                else 
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("| " + row); 
                }
                Console.WriteLine();
            }

            // For the styling of the board for all other rows
            while (counter <= gameBoard.Size)
            {
                if (counter == gameBoard.Size) 
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("+"); 
                }
                else 
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("+---"); 
                }
                counter++;
            }
            counter = 0;
        }

        // Method that handles gameEnd
        static public void EndGameStatus()
        {
            // If game end is true check status of true
            // If the user selects a bomb -> fail message + reset game
            // If the user reveals all bombs -> success message + reset game
        }
    }
}
