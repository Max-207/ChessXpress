using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Controls;
using ChessXpress;
using NSFigure;

namespace NSQueen
{
    internal class Queen : Chessfigure
    {
        public Queen(string name, string color, int posx, int posy, ImageSource img)
            : base(name, color, posx, posy, img)
        {
            Name = name;
            Color = color;
            PositionX = posx;
            PositionY = posy;
            Image = img;
        }

        public bool checkMove(CustomData sb2Data)
        {
            int x_move = sb2Data.X - PositionX;
            int y_move = sb2Data.Y - PositionY;

            if (x_move == 0 || y_move == 0) return checkStraight(x_move, y_move);
            else if (Math.Abs(x_move) == Math.Abs(y_move)) return checkSkew(x_move, y_move);
            MainWindow.selectedButton1 = null;
            return false;
        }

        private bool checkStraight(int x_move, int y_move)
        {
            if (x_move > 0)
            {
                for (int i = PositionX + 1; i < (PositionX + x_move); i++)
                {
                    if (MainWindow.Buttons[PositionY, i].Content != null)
                    {
                        MainWindow.selectedButton1 = null;
                        return false;
                    }
                }
            }
            else if (x_move < 0)
            {
                for (int i = PositionX - 1; i > (PositionX + x_move); i--)
                {
                    if (MainWindow.Buttons[PositionY, i].Content != null)
                    {
                        MainWindow.selectedButton1 = null;
                        return false;
                    }
                }
            }
            else if (y_move > 0)
            {
                for (int i = PositionY + 1; i < (PositionY + y_move); i++)
                {
                    if (MainWindow.Buttons[i, PositionX].Content != null)
                    {
                        MainWindow.selectedButton1 = null;
                        return false;
                    }
                }
            }
            else if (y_move < 0)
            {
                for (int i = PositionY - 1; i > (PositionY + y_move); i--)
                {
                    if (MainWindow.Buttons[i, PositionX].Content != null)
                    {
                        MainWindow.selectedButton1 = null;
                        return false;
                    }
                }
            }
            MainWindow.last_move = Color[0];
            return true;
        }

        private bool checkSkew(int x_move, int y_move)
        {
            if (x_move > 0 && y_move > 0)
            {
                int j = PositionY + 1;
                for (int i = PositionX + 1; i < (PositionX + x_move); i++)
                {
                    if (MainWindow.Buttons[j, i].Content != null)
                    {
                        MainWindow.selectedButton1 = null;
                        return false;
                    }
                    j++;
                }
            }
            else if (x_move > 0 && y_move < 0)
            {
                int j = PositionY - 1;
                for (int i = PositionX + 1; i < (PositionX + x_move); i++)
                {
                    if (MainWindow.Buttons[j, i].Content != null)
                    {
                        MainWindow.selectedButton1 = null;
                        return false;
                    }
                    j--;
                }
            }
            else if (x_move < 0 && y_move > 0)
            {
                int j = PositionY + 1;
                for (int i = PositionX - 1; i > (PositionX + x_move); i--)
                {
                    if (MainWindow.Buttons[j, i].Content != null)
                    {
                        MainWindow.selectedButton1 = null;
                        return false;
                    }
                    j++;
                }
            }
            else if (x_move < 0 && y_move < 0)
            {
                int j = PositionY - 1;
                for (int i = PositionX - 1; i > (PositionX + x_move); i--)
                {
                    if (MainWindow.Buttons[j, i].Content != null)
                    {
                        MainWindow.selectedButton1 = null;
                        return false;
                    }
                    j--;
                }
            }
            MainWindow.last_move = Color[0];
            return true;
        }
    }
}
