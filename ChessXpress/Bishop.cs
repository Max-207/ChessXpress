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

namespace NSBishop
{
    internal class Bishop : Chessfigure
    {
        public Bishop(string name, string color, int posx, int posy, ImageSource img)
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

            if (Math.Abs(x_move) == Math.Abs(y_move))
            {

                //rechts unten
                if(x_move > 0 && y_move > 0)
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
                //rechts oben
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
                //links unten
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

                //links oben
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
            MainWindow.selectedButton1 = null;
            return false;
        }
    }
}
