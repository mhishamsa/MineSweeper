using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class MineField
    {

        string PlayerName;

        public int Rows;
        public int Cols;
        public int Mines;

        public int Clicks;

        public int Score;
        public int Time;

        public Cell[,] Field;

        public MineField(string Name, int Rows, int Cols, int Mines)
        {
            PlayerName = Name;
            this.Rows = Rows;
            this.Cols = Cols;
            this.Mines = Mines;

            Clicks = 0;
            Score = 0;
            Field = new Cell[Rows,Cols];

            for(int i = 0; i < Rows; i++)               //set coordinates for each cell
            {
                for(int j = 0; j < Cols; j++)
                {
                    Field[i,j] = new Cell();
                    Field[i, j].RowIndex = i;
                    Field[i, j].ColIndex = j;
                }
            }
        }

        public void RandomizeMines(Cell TheChosenOne)
        {
            Random rand = new Random();
            int RandRow;
            int RandCol;

            for(int i = 0; i < Mines; i++)
            {
                RandRow = rand.Next(0, Rows - 1);
                RandCol = rand.Next(0, Cols - 1);

                while (Field[RandRow, RandCol].Mine == true || (RandRow == TheChosenOne.RowIndex && RandCol == TheChosenOne.ColIndex))
                {
                    RandRow = rand.Next(0, Rows);
                    RandCol = rand.Next(0, Cols);
                }

                Field[RandRow, RandCol].Mine = true;
            }

        }

        public void CountCellMines()
        {

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    int mines = 0;

                    Cell UL = null;         //Up Left
                    Cell U = null;          //Up 
                    Cell UR = null;         //Up Right
                    Cell R = null;          //Right
                    Cell L = null;
                    Cell DL = null;
                    Cell D = null;
                    Cell DR = null;

                    if (i > 0 && j > 0)
                    {
                        UL = Field[i - 1, j - 1];
                    }

                    if (i > 0)
                    {
                        U = Field[i - 1, j];
                    }

                    if (i > 0 && j < Cols - 1)
                    {
                        UR = Field[i - 1, j + 1];
                    }

                    if (j < Cols - 1)
                    {
                        R = Field[i, j + 1];
                    }

                    if (j > 0)
                    {
                        L = Field[i, j - 1];
                    }

                    if (i < Rows - 1 && j > 0)
                    {
                        DL = Field[i + 1, j - 1];
                    }

                    if (i < Rows - 1)
                    {
                        D = Field[i + 1, j];
                    }

                    if (i < Rows - 1 && j < Cols - 1)
                    {
                        DR = Field[i + 1, j + 1];
                    }


                    if (UL != null && UL.Mine)
                    {
                        mines++;
                    }

                    if (U != null && U.Mine)
                    {
                        mines++;
                    }

                    if (UR != null && UR.Mine)
                    {
                        mines++;
                    }

                    if (R != null && R.Mine)
                    {
                        mines++;
                    }

                    if (L != null && L.Mine)
                    {
                        mines++;
                    }

                    if (DL != null && DL.Mine)
                    {
                        mines++;
                    }

                    if (DR != null && DR.Mine)
                    {
                        mines++;
                    }

                    if (D != null && D.Mine)
                    {
                        mines++;
                    }

                    Field[i,j].MinesAround = mines;

                    switch (mines)
                    {
                        case 1:
                            Field[i, j].Icon = "../Images/Numbers/Number-1-icon.png";
                            break;
                        case 2:
                            Field[i, j].Icon = "../Images/Numbers/Number-2-icon.png";
                            break;
                        case 3:
                            Field[i, j].Icon = "../Images/Numbers/Number-3-icon.png";
                            break;
                        case 4:
                            Field[i, j].Icon = "../Images/Numbers/Number-4-icon.png";
                            break;
                        case 5:
                            Field[i, j].Icon = "../Images/Numbers/Number-5-icon.png";
                            break;
                        case 6:
                            Field[i, j].Icon = "../Images/Numbers/Number-6-icon.png";
                            break;
                        case 7:
                            Field[i, j].Icon = "../Images/Numbers/Number-7-icon.png";
                            break;
                        case 8:
                            Field[i, j].Icon = "../Images/Numbers/Number-8-icon.png";
                            break;
                        case 9:
                            Field[i, j].Icon = "../Images/Numbers/Number-9-icon.png";
                            break;
                        default:
                            Field[i, j].Icon = "";
                            break;
                    }
                }
            }
        }
    }
}
