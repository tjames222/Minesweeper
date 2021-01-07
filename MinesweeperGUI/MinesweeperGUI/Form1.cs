using MinesweeperClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperGUI
{
    public partial class Form1 : Form
    {
        static public int size = 20;
        static public int difficulty = 1;
        static public Board gameBoard = new Board(size);
        public Button[,] btnGrid = new Button[gameBoard.Size, gameBoard.Size];
        public Stopwatch timer = new Stopwatch();
        static public bool gameEnd { get; set; }
        static public bool allVisited = false;
        static public int row;
        static public int col;


        public Form1(int diff, int s)
        {
            // Timer starts now       
            timer.Start();

            // Difficulty paramater passed from radio button on level form
            difficulty = diff;

            // Game board size passed from level form
            size = s;

            InitializeComponent();

            // Calls the Board.setupLiveNeighbors command
            gameBoard.SetupLiveNeighbors(difficulty, size);

            // Calls the Board.calculateLiveNeighbors command
            gameBoard.CalculateLiveNeighbors();

            // Populates the gameboard with buttons
            populateGrid();
           
        }

        public void PlayGame()
        {
            // If game end is true check status of true
            // If the user selects a bomb -> fail message
            // If the user reveals all bombs -> success message

            int userRow;
            int userCol;

            // Button click row value                   
            userRow = row;

            // Button click col value
            userCol = col; 

            // Check to see if there is a bomb
            if (gameBoard.Grid[userRow, userCol].Live == true)
            {
                gameEnd = true; // Set game end parameter to true
                timer.Stop(); // Stop the timer
                updateButtonLabels(); // Reveal board

                // Display game over message + time elapsed
                MessageBox.Show("            GAME OVER                     " +
                        "\nGame Time: " + timer.Elapsed.Minutes + " Minutes and " +
                        timer.Elapsed.Seconds + " seconds");
                Application.Exit(); // Stops the program
            }

            // Set the user selection Visited to true
            else
            {
                if (gameBoard.Grid[userRow, userCol].NeighborsLive == 0)
                {
                    gameBoard.FloodFill(userRow, userCol); // Checks for other empty cells with no live neighbors
                }
                gameBoard.Grid[userRow, userCol].Visited = true; // Set current selection Cell Visited to true

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
                    gameEnd = true; // End game parameter set to true
                    timer.Stop(); // Stop the timer
                    updateButtonLabels(); // Reveal board

                    // Display user win message + time elapsed
                    MessageBox.Show("            You Win!                  " +
                        "\nGame Time: " + timer.Elapsed.Minutes + " Minutes and " +
                        timer.Elapsed.Seconds + " seconds");
                    Application.Exit(); // Stop the program
                }
            }
        }

        public void populateGrid()
        {
            // This fuction will fill the panel1 control with buttons.
            int buttonSize = 30; // Calculate the width of each button on the Grid
            panel1.Height = panel1.Width; // Set the grid to be square

            // Create buttons and place them in the panel
            for (int r = 0; r < gameBoard.Size; r++)
            {
                for (int c = 0; c < gameBoard.Size; c++)
                {
                    btnGrid[r, c] = new Button();

                    // Make each button square
                    btnGrid[r, c].Width = buttonSize;
                    btnGrid[r, c].Height = buttonSize;

                    btnGrid[r, c].MouseDown += Grid_Button_Click; // Add the same click event to each button.
                    panel1.Controls.Add(btnGrid[r, c]); // place the button on the panel
                    btnGrid[r, c].Location = new Point(buttonSize * r, buttonSize * c); // Position it in x, y
                 
                    // For testing purposes. Remove later.
                    //btnGrid[r, c].Text = r.ToString() + "|" + c.ToString();

                    // The tag attribute will hold the row and column number in a string
                    btnGrid[r, c].Tag = r.ToString() + "|" + c.ToString();
                }
            }
        }

        private void Grid_Button_Click(object sender, MouseEventArgs e)
        {
            Image flag = Image.FromFile(@"C:\Users\TJames42\source\repos\C#\MinesweeperGUI\flag.png");

            // Get the row and Column number of the button just clicked
            string[] strArr = (sender as Button).Tag.ToString().Split('|');
            row = int.Parse(strArr[0]);
            col = int.Parse(strArr[1]);

            // Right click handler
            if (e.Button == MouseButtons.Right)
            {
                // Sets a flag
                btnGrid[row, col].Image = flag;
            } 
            // Left click or Middle click handler
            else
            {
                // Clears flag if a flag was placed
                btnGrid[row, col].Image = null;

                // Calls the play game method
                PlayGame();
                updateButtonLabels();

                // Set the background color of the clicked button to something different.
                (sender as Button).BackColor = Color.Gray;
            }
        }

       public void updateButtonLabels()
        {
            Image bomb = Image.FromFile(@"C:\Users\TJames42\source\repos\C#\MinesweeperGUI\bomb.jpg");                       
            Image flag = Image.FromFile(@"C:\Users\TJames42\source\repos\C#\MinesweeperGUI\flag.png");

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {                   
                    btnGrid[r, c].Text = "";
                
                    if (gameBoard.Grid[r, c].Visited == true) // Live neighbors to text
                    {
                        btnGrid[r, c].BackColor = Color.LightGray;
                        btnGrid[r, c].Text = gameBoard.Grid[r, c].NeighborsLive.ToString();
                    }
                    else if (allVisited == false && gameEnd == true) // Lose game shows Bombs
                    {
                        btnGrid[r, c].BackColor = Color.LightGray;
                        btnGrid[r, c].Text = gameBoard.Grid[r, c].NeighborsLive.ToString();
                        if (gameBoard.Grid[r, c].Live == true) { btnGrid[r, c].Image = bomb; }
                        
                    }
                    else if (allVisited == true && gameEnd == true) // Win Game shows flags
                    {
                        btnGrid[r, c].BackColor = Color.LightGray;
                        if (gameBoard.Grid[r, c].Live == true) { btnGrid[r, c].Image = flag; }
                        if (btnGrid[r, c].Image != flag)
                        {
                            btnGrid[r, c].Text = gameBoard.Grid[r, c].NeighborsLive.ToString();
                        }                                           
                    }
                }           
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
