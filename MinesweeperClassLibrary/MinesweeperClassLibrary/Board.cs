using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace MinesweeperClassLibrary
{
    public class Board
    {
        // Properties 
        public int Size { get; set; }
        public Cell[,] Grid { get; set; }

        // Beginner = 1 (33%), Intermediate = 2 (66%), Expert =3 (99%)
        public double Difficulty { get; set; }

        // Constructor with one parameter
        public Board(int size)
        {
            Size = size;

            Grid = new Cell[Size, Size];

            // Loading the Grid with Cell objects
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++) 
                {
                    Grid[row, col] = new Cell(row, col, false, false, 0);
                }
            }
        }


        public void SetupLiveNeighbors(int difficulty, int size)
        {
            Difficulty = difficulty;
            Size = size;

            // If difficulty is 1 then logic would be Size * .33
            // If difficulty is 2 then logic would be Size * .66
            // If difficulty is 3 then logic would be Size * .99

            double diffPercent = 0;

            switch (Difficulty)
            {
                case 1:
                    diffPercent = .33;
                    break;
                case 2:
                    diffPercent = .66;
                    break;
                case 3:
                    diffPercent = .99;
                    break;
            }

            // Used to randomly select tiles
            Random rand = new Random();

            int numberOfMines = Convert.ToInt32((Size * diffPercent) * 2);         

            // Loads mines into random cells
            while (numberOfMines > 0)
            {
                int randRow = rand.Next(Size);
                int randCol = rand.Next(Size);
                
                Grid[randRow, randCol].Live = true;
                numberOfMines--;
            }
        }

        public void CalculateLiveNeighbors()
        {
            // Read all coordinates around a given cell
            // Determine if a neighbor cell is Live = true
            // If Neighbor cell is Live = true -> LiveNeighborCount++ -> continue through loop. else check next neighbor
            // If current cell is Live = true -> set LiveNeighborCount = 9
            // Set Grid[row,col].neighborsLive value

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    try
                    {
                        if (Grid[row - 1, col - 1].Live == true)
                        {
                            Grid[row, col].NeighborsLive++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Grid[row, col - 1].Live == true)
                        {
                            Grid[row, col].NeighborsLive++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Grid[row + 1, col - 1].Live == true)
                        {
                            Grid[row, col].NeighborsLive++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    try {
                        if (Grid[row + 1, col].Live == true)
                        {
                            Grid[row, col].NeighborsLive++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Grid[row + 1, col + 1].Live == true)
                        {
                            Grid[row, col].NeighborsLive++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Grid[row, col + 1].Live == true)
                        {
                            Grid[row, col].NeighborsLive++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Grid[row - 1, col + 1].Live == true)
                        {
                            Grid[row, col].NeighborsLive++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Grid[row - 1, col].Live == true)
                        {
                            Grid[row, col].NeighborsLive++;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Grid[row, col].Live == true)
                        {
                            Grid[row, col].NeighborsLive = 9;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                }
                
            }

        }

        public void FloodFill(int row, int col)
        {
            // Logic
            // If Cells around selected Cell are not live and do not have any liveNeighbors
            // Set Grid.Visited to true.

            // Escape the loop 
            if (Grid[row, col].Visited == true) { return; }

            Grid[row, col].Visited = true;

            // Start recursion
            try { if (Grid[row, col + 1].NeighborsLive == 0) { FloodFill(row, col + 1); } } // Right
            catch (IndexOutOfRangeException) { }

            try { if (Grid[row - 1, col].NeighborsLive == 0) { FloodFill(row - 1, col); } } // Down
            catch (IndexOutOfRangeException) { }

            try { if (Grid[row, col - 1].NeighborsLive == 0) { FloodFill(row, col - 1); } } // Left
            catch (IndexOutOfRangeException) { }

            try { if (Grid[row + 1, col].NeighborsLive == 0) { FloodFill(row + 1, col); } } // Up
            catch (IndexOutOfRangeException) { }

            if (Grid[row, col].NeighborsLive == 0)
            {
                try { Grid[row, col + 1].Visited = true; }
                catch (IndexOutOfRangeException) { }
                try { Grid[row - 1, col].Visited = true; }
                catch (IndexOutOfRangeException) { }
                try { Grid[row, col - 1].Visited = true; }
                catch (IndexOutOfRangeException) { }
                try { Grid[row + 1, col].Visited = true; }
                catch (IndexOutOfRangeException) { }
            }

            /* Minesweeper does not check corners
            try { if (Grid[row + 1, col + 1].NeighborsLive == 0) { FloodFill(row + 1, col + 1); } } // Up and Right
            catch (IndexOutOfRangeException) { }

            try { if (Grid[row - 1, col + 1].NeighborsLive == 0) { FloodFill(row - 1, col + 1); } } // Down and Right
            catch (IndexOutOfRangeException) { }

            try { if (Grid[row + 1, col - 1].NeighborsLive == 0) { FloodFill(row + 1, col - 1); } } // Up and Left
            catch (IndexOutOfRangeException) { }

            try { if (Grid[row - 1, col - 1].NeighborsLive == 0) { FloodFill(row - 1, col - 1); } } // Down and Left
            catch (IndexOutOfRangeException) { }
            */
        }
    }
}
