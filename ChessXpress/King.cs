using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Controls;
using System.Threading;
using ChessXpress;
using NSFigure;
using NSRock;
using NSPawn;
using System.Collections;

namespace NSKing
{
    internal class King : Chessfigure
    {
        public bool moved = false;
        public bool isCheck;
        public List<(Button, Rock)> CastlingButtons = new List<(Button, Rock)>();
        public bool moves_smallCastling = false;
        public bool moves_bigCastling = false;
        public bool moveIsTestMove = false;

        public King(string name, string color, int posx, int posy, ImageSource img)
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
            moveIsTestMove = isTestMove;
            if (!(PositionX + 1 < sb2Data.X || PositionX - 1 > sb2Data.X
                || PositionY + 1 < sb2Data.Y || PositionY - 1 > sb2Data.Y))
            {
                MainWindow.last_move = Color[0];
                return true;
            }
            else if (castling(sb2Data))
            {
                MainWindow.last_move = Color[0];
                return true;
            }
            MainWindow.selectedButton1 = null;
            return false;
        }

        private bool castling(CustomData sb2Data)
        {
            if (isCheck || moved) return false;
            return sb2Data.X > PositionX ? castling_small(sb2Data) : castling_big(sb2Data);
        }
        private bool castling_small(CustomData sb2Data)
        {
            Button rock = Color == "white" ? MainWindow.Buttons[7, 7] : MainWindow.Buttons[0, 7];
            CustomData rockData = (CustomData)rock.Tag;
            CustomData nextToRockFig1 = (CustomData)MainWindow.Buttons[rockData.Y, rockData.X - 1].Tag;
            CustomData nextToRockFig2 = (CustomData)MainWindow.Buttons[rockData.Y, rockData.X - 2].Tag;
            if (Color[0] != rockData.figure[0]) return false;
            Button castlingBtn;
            if (sb2Data.X < 2 && !((CustomData)MainWindow.Buttons[sb2Data.Y, 7].Tag).figure.Contains("rock1")) return false;
            castlingBtn = rock;
            if (!CastlingButtons.Any(item => item.Item1.Equals(castlingBtn))) return false;
            if ((nextToRockFig1.figure != "empty") || Check.checkIfCheck_init(PositionX + 1, PositionY, Color)
                || (nextToRockFig2.figure != "empty")) return false;
            moves_smallCastling = true;
            return true;
        }

        private bool castling_big(CustomData sb2Data)
        {
            Button rock = Color == "white" ? MainWindow.Buttons[7, 0] : MainWindow.Buttons[0, 0];
            CustomData rockData = (CustomData)rock.Tag;
            CustomData nextToRockFig1 = (CustomData)MainWindow.Buttons[rockData.Y, rockData.X + 1].Tag;
            CustomData nextToRockFig2 = (CustomData)MainWindow.Buttons[rockData.Y, rockData.X + 2].Tag;
            CustomData nextToRockFig3 = (CustomData)MainWindow.Buttons[rockData.Y, rockData.X + 3].Tag;

            if (Color[0] != rockData.figure[0]) return false;
            Button castlingBtn;
            if (sb2Data.X < 3 && !((CustomData)MainWindow.Buttons[sb2Data.Y, 0].Tag).figure.Contains("rock1")) return false;
            castlingBtn = rock;
            if (!CastlingButtons.Any(item => item.Item1.Equals(castlingBtn))) return false;
            if ((nextToRockFig1.figure != "empty") || Check.checkIfCheck_init(PositionX -1, PositionY, Color)
                || (nextToRockFig2.figure != "empty") || (nextToRockFig3.figure != "empty")) return false;
            moves_bigCastling = true;
            return true;
        }

        public void move_run_smallCastling(CustomData sb2Data)
        {
            (Button, Rock) itemToRemove = (null, null);
            foreach ((Button, Rock) item in CastlingButtons)
            {
                if(item.Item2.PositionX == 7 && item.Item2.PositionY == sb2Data.Y)
                {
                    itemToRemove = item;
                    ((CustomData)item.Item1.Tag).figure = "empty";
                    item.Item2.PositionX -= 2;
                    ((CustomData)MainWindow.Buttons[sb2Data.Y, item.Item2.PositionX].Tag).figure = item.Item2.Name;
                    break;
                }
            }
            ((CustomData)MainWindow.Buttons[PositionY, PositionX].Tag).figure = "empty";
            PositionX += 2;
            ((CustomData)MainWindow.Buttons[sb2Data.Y, PositionX].Tag).figure = Name;
            if(!moveIsTestMove)CastlingButtons.Remove(itemToRemove);
        }
        public void move_run_bigCastling(CustomData sb2Data)
        {
            (Button, Rock) itemToRemove = (null, null);
            foreach ((Button, Rock) item in CastlingButtons)
            {
                if (item.Item2.PositionX == 0 && item.Item2.PositionY == sb2Data.Y)
                {
                    itemToRemove = item;
                    ((CustomData)item.Item1.Tag).figure = "empty";
                    item.Item2.PositionX += 3;
                    ((CustomData)MainWindow.Buttons[sb2Data.Y, item.Item2.PositionX].Tag).figure = item.Item2.Name;
                    break;
                }
            }
            ((CustomData)MainWindow.Buttons[PositionY, PositionX].Tag).figure = "empty";
            PositionX -= 2;
            ((CustomData)MainWindow.Buttons[sb2Data.Y, PositionX].Tag).figure = Name;
            if (!moveIsTestMove) CastlingButtons.Remove(itemToRemove);
        }
    }
}
