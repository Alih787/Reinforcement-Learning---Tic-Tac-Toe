using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TicTacToe
{
    static class Grid
    {
        /**grid
         * 0 1 2
         * 3 4 5
         * 6 7 8
         * **/

        public static string[] grid = new string[9];
        public static object[] buttons = new object[9];

        public static bool CheckWinX()
        {
            if ((grid[0] == "X" && grid[1] == "X" && grid[2] == "X")||
                (grid[3] == "X" && grid[4] == "X" && grid[5] == "X") ||
                (grid[6] == "X" && grid[7] == "X" && grid[8] == "X") ||//rows horizontal
                (grid[0] == "X" && grid[3] == "X" && grid[6] == "X") ||
                (grid[1] == "X" && grid[4] == "X" && grid[7] == "X") ||
                (grid[2] == "X" && grid[5] == "X" && grid[8] == "X") ||//rows vertical
                (grid[0] == "X" && grid[4] == "X" && grid[8] == "X") ||
                (grid[2] == "X" && grid[4] == "X" && grid[6] == "X"))//rows diagonal
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckWinO()
        {
            if ((grid[0] == "O" && grid[1] == "O" && grid[2] == "O") ||
                (grid[3] == "O" && grid[4] == "O" && grid[5] == "O") ||
                (grid[6] == "O" && grid[7] == "O" && grid[8] == "O") ||//rows horizontal
                (grid[0] == "O" && grid[3] == "O" && grid[6] == "O") ||
                (grid[1] == "O" && grid[4] == "O" && grid[7] == "O") ||
                (grid[2] == "O" && grid[5] == "O" && grid[8] == "O") ||//rows vertical
                (grid[0] == "O" && grid[4] == "O" && grid[8] == "O") ||
                (grid[2] == "O" && grid[4] == "O" && grid[6] == "O"))//rows diagonal
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckDraw()
        {
            int filled = 0;
            for(int i = 0; i < grid.Length; i++)
            {
                if(grid[i] !=string.Empty)
                {
                    filled++;
                }
            }
            if (filled == 9 && !CheckWinO() && !CheckWinX())
            {
                Link.Logs.Input("Draw");
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckEnd()
        {
            if (CheckDraw() || CheckWinO() || CheckWinX())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Reset()
        {
            for(int i = 0; i < grid.Length; i++)
            {
                grid[i] = string.Empty;
            }
        }

        public static void Updategrid()
        {
            if (Link.Main.Training)
            {
                return;
            }
            for(int i = 0;i<buttons.Length;i++)
            {
                ((Button)buttons[i]).Text = grid[i];
                if (((Button)buttons[i]).Text != string.Empty)
                {
                    ((Button)buttons[i]).Enabled = false;
                }
                else
                {
                    ((Button)buttons[i]).Enabled = true;
                }
            }
        }
    }
}
