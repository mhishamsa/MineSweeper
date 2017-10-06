using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class Cell
    {
        public bool Mine;
        public bool Visited;
        public bool Flagged;

        public int RowIndex;
        public int ColIndex;

        public int MinesAround;

        public string Icon;

        public Cell()
        {
            Mine = false;
            Visited = false;
            Flagged = false;
            MinesAround = 0;
        }
    }
}
