using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperClassLibrary
{
    public class Cell
    {
        // Properties 
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Visited { get; set; }
        public bool Live { get; set; }
        public int NeighborsLive { get; set; }

        // Constructor that accepts parameters
        public Cell(int row, int column, bool visited, bool live, int neighborsLive)
        {
            Row = row;
            Column = column;
            Visited = visited;
            Live = live;
            NeighborsLive = neighborsLive;
        }

        // Constructor that has 0 parameters. Preset values are provided.
        public Cell()
        {
            Row = -1;
            Column = -1;
            Visited = false;
            Live = false;
            NeighborsLive = 0;
        }
    }
}
