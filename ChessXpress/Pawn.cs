using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using System.Timers;
using ChessXpress;
using NSFigure;
using System.Windows;

namespace NSPawn
{
    internal class Pawn : Chessfigure
    {
        public bool EnPassentEnabled = false;
        public Button enPassentBtn = new Button();
        public bool moves_enPassent = false;

        public Pawn(string name, string color, int posx, int posy, ImageSource img)
            : base(name, color, posx, posy, img)
        {
            Name = name;
            Color = color;
            PositionX = posx;
            PositionY = posy;
            Image = img;
        }
        
        public bool checkMove(CustomData sb2Data, bool isTestMove)
        {
            int pawn_move1 = Color == "white" ? 1 : (Color == "black" ? -1 : 0);
            int pawn_move2 = Color == "white" ? 2 : (Color == "black" ? -2 : 0);
            int pawn_startposy = Color == "white" ? 6 : (Color == "black" ? 1 : 0);

            if (sb2Data.figure == "empty")
            {
                if ((PositionY == sb2Data.Y + pawn_move1) && (PositionX == sb2Data.X))
                {
                    if (PositionX == sb2Data.X)
                    {
                        MainWindow.last_move = sb2Data.figure[0];
                        return true;
                    }
                }
                else if ((PositionY == pawn_startposy) && (PositionY == sb2Data.Y + pawn_move2) && (PositionX == sb2Data.X))
                {
                    if (PositionX == sb2Data.X)
                    {
                        EnPassentEnabled = isTestMove ? false : true;
                        MainWindow.last_move = sb2Data.figure[0];
                        return true;
                    }
                }
                else
                {
                    if(PositionY == sb2Data.Y + pawn_move1)
                    {
                        int pawn_moveX = PositionX == sb2Data.X - 1 ? 1 : (PositionX == sb2Data.X + 1 ? -1 : 0);
                        if (pawn_moveX == 0) return false;

                        enPassentBtn = MainWindow.Buttons[PositionY, PositionX + pawn_moveX];
                        if ((Color == "white" ? blackEPButtons : whiteEPButtons).Contains((CustomData)enPassentBtn.Tag)
                            && Color[0] != ((CustomData)enPassentBtn.Tag).figure[0])
                        {
                            moves_enPassent = true;
                            MainWindow.last_move = sb2Data.figure[0];
                            return true;
                        }
                    }
                }
            }
            else if ((sb2Data.figure[0] == 'w' && Color[0] == 'b') || (sb2Data.figure[0] == 'b' && Color[0] == 'w'))
            {
                if (PositionY == sb2Data.Y + pawn_move1)
                {
                    if ((sb2Data.X + 1 < 8 && PositionX == sb2Data.X + 1)
                        ||(sb2Data.X - 1 >= 0 && PositionX == sb2Data.X - 1))
                    {
                        MainWindow.last_move = sb2Data.figure[0];
                        return true;
                    }
                }
            }
            MainWindow.selectedButton1 = null;
            return false;
        }
    }
}
