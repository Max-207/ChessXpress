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


namespace NSKnight
{
    internal class Knight : Chessfigure
    {
        public Knight(string name, string color, int posx, int posy, ImageSource img)
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
            int x_move = +Math.Abs(sb2Data.X - PositionX);
            int y_move = +Math.Abs(sb2Data.Y - PositionY);

            if ((x_move == 2 && y_move == 1) || (y_move == 2 && x_move == 1))
            {
                MainWindow.last_move = Color[0];
                return true;
            }
            MainWindow.selectedButton1 = null;
            return false;
        }
    }
}
