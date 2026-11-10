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
using NSPawn;
using NSBishop;
using NSKing;
using NSKnight;
using NSQueen;
using NSRock;
using System.Collections;


namespace NSFigure
{
    internal class Chessfigure
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public ImageSource Image { get; set; }
        public static List<CustomData> whiteEPButtons = new List<CustomData>();
        public static List<CustomData> blackEPButtons = new List<CustomData>();


        public Chessfigure(string name, string color, int posx, int posy, ImageSource img)
        {
            Name = name;
            Color = color;
            PositionX = posx;
            PositionY = posy;
            Image = img;
        }

        public bool move(King selfking, bool isTestMove)
        {
            bool isKing = false;
            int kposx;
            int kposy;
            if(this is King thisking)
            {
                kposx = selfking.PositionX;
                kposy = selfking.PositionY;
                selfking = thisking;
                isKing = true;
            }
            else
            {
                kposx = selfking.PositionX;
                kposy = selfking.PositionY;
            }
            bool isPawn = false;
            Pawn pawn = null;
            Button rockBtn = null;
            if (this is Pawn thispawn)
            {
                isPawn = true;
                pawn = thispawn;
            }
            else if(this is Rock)
            {
                rockBtn = MainWindow.Buttons[PositionY, PositionX];
;           }
            //Bin ich dran?
            if (!((Color == "white" && MainWindow.w_timer.Enabled)
                || (Color == "black" && MainWindow.b_timer.Enabled)))
            {
                MainWindow.selectedButton1 = null;
                MainWindow.selectedButton2 = null;
                return false;
            }

            //Move ausführen
            CustomData sb1Data = (CustomData)MainWindow.selectedButton1.Tag;
            CustomData sb2Data = (CustomData)MainWindow.selectedButton2.Tag;

            if (!start_checkMove(sb2Data, isTestMove)) return false;

            //Abspeichern der geschlagenen Figur für Reset
            CustomData btnToSave = sb2Data.figure != "empty" ? sb2Data : (isPawn && pawn.moves_enPassent ? (CustomData)pawn.enPassentBtn.Tag : sb2Data);
            if(btnToSave.figure != "empty")
            {
                foreach (var btn_fig in btnToSave.figure[0] == 'w' ? MainWindow.whiteFigures : (btnToSave.figure[0] == 'b' ? MainWindow.blackFigures : null))
                {
                    if (btn_fig.Item2 == btnToSave.figure)
                    {
                        if (!isTestMove) MainWindow.beaten_figure = btn_fig.Item1;
                        else MainWindow.beaten_figure_test = btn_fig.Item1;
                        break;
                    }
                }
            }
            
            if (Color == "white" && btnToSave.figure != "empty") MainWindow.blackFigures.RemoveAll(entry => entry.Item2 == btnToSave.figure);
            else if (Color == "black" && btnToSave.figure != "empty") MainWindow.whiteFigures.RemoveAll(entry => entry.Item2 == btnToSave.figure);
            //Move technisch ausführen
            if (!(isKing && (selfking.moves_smallCastling || selfking.moves_bigCastling))) move_run(sb1Data, sb2Data);
            else if (selfking.moves_smallCastling) selfking.move_run_smallCastling(sb2Data);
            else if (selfking.moves_bigCastling) selfking.move_run_bigCastling(sb2Data);
            if (isKing)
            {
                kposx = PositionX;
                kposy = PositionY;
            }

            //Prüfen ob der Move möglich ist (Schachgebot?)
            if (Check.checkIfCheck_init(kposx, kposy, Color))
            {
                if(!isTestMove) selfking.isCheck = true;
                resetMove(sb1Data, sb2Data, isTestMove);
                return false;
            }

            if (isTestMove)
            {
                if (isKing)
                {
                    selfking.moves_smallCastling = false;
                    selfking.moves_bigCastling = false;
                }
                return true;
            }
            //Move grafisch ausführen & Sonderzüge beachten
            if (isPawn && pawn.enPassentBtn != null) pawn.enPassentBtn.Content = null;
            else if(rockBtn != null && !isTestMove && selfking.CastlingButtons.Any(item => item.Item1.Equals(rockBtn))) selfking.CastlingButtons.RemoveAll(item => item.Item1.Equals(rockBtn));

            if(!(isKing && (selfking.moves_smallCastling || selfking.moves_bigCastling)))
            {
                MainWindow.selectedButton2.Content = new Image { Source = Image };
                MainWindow.selectedButton1.Content = null;
            }
            else if(isKing && selfking.moves_smallCastling)
            {
                MainWindow.Buttons[kposy, kposx].Content = new Image { Source = Image };
                MainWindow.selectedButton1.Content = null;
                Image img = Color == "white" ? MainWindow.wrockIMG : MainWindow.brockIMG;
                MainWindow.Buttons[kposy, 5].Content = new Image { Source = img.Source };
                MainWindow.Buttons[kposy, 7].Content = null;
            }
            else if (isKing && selfking.moves_bigCastling)
            {
                MainWindow.Buttons[kposy, kposx].Content = new Image { Source = Image };
                MainWindow.selectedButton1.Content = null;
                Image img = Color == "white" ? MainWindow.wrockIMG : MainWindow.brockIMG;
                MainWindow.Buttons[kposy, 3].Content = new Image { Source = img.Source };
                MainWindow.Buttons[kposy, 0].Content = null;
            }
            if (isKing)
            {
                selfking.moves_smallCastling = false;
                selfking.moves_bigCastling = false;
            }

            (Color == "white" ? MainWindow.whiteFigures : MainWindow.blackFigures).ForEach(item =>
            {
                if (item.Item1 is Pawn) (Color == "white" ? whiteEPButtons : blackEPButtons).Clear();
            });
            if (isPawn && pawn.EnPassentEnabled)
            {
                (Color == "white" ? whiteEPButtons : blackEPButtons).Add((CustomData)MainWindow.Buttons[PositionY, PositionX].Tag);
                pawn.EnPassentEnabled = false;
            }
            MainWindow.selectedButton1 = null;
            MainWindow.selectedButton2 = null;
            return true;
        }
        private bool start_checkMove(CustomData sb2Data, bool isTestMove)
        {
            if (this is Pawn pawn) return pawn.checkMove(sb2Data, isTestMove);
            else if (this is King king) return king.checkMove(sb2Data, isTestMove);
            else if (this is Queen queen) return queen.checkMove(sb2Data);
            else if (this is Rock rock) return rock.checkMove(sb2Data);
            else if (this is Bishop bishop) return bishop.checkMove(sb2Data);
            else if (this is Knight knight) return knight.checkMove(sb2Data);
            return false;
        }
        public void resetMove(CustomData sb1Data, CustomData sb2Data, bool isTestMove)
        {
            Pawn pawn = null;
            bool isPawn = false;
            if (this is Pawn thisPawn)
            {
                pawn = thisPawn;
                isPawn = true;
            }
            string btn_fig_name;
            if ((MainWindow.beaten_figure == null && !isTestMove) || (MainWindow.beaten_figure_test == null && isTestMove)) btn_fig_name = "empty";
            else
            {
                if (!isTestMove && MainWindow.beaten_figure != null) btn_fig_name = ((Chessfigure)MainWindow.beaten_figure).Name;
                else btn_fig_name = ((Chessfigure)MainWindow.beaten_figure_test).Name;
            }

            PositionX = sb1Data.X;
            PositionY = sb1Data.Y;
            ((CustomData)MainWindow.selectedButton1.Tag).figure = sb2Data.figure;
            if(isPawn && pawn.moves_enPassent)
            {
                ((CustomData)MainWindow.selectedButton2.Tag).figure = "empty";
                ((CustomData)((Pawn)this).enPassentBtn.Tag).figure = btn_fig_name;
            }
            else ((CustomData)MainWindow.selectedButton2.Tag).figure = btn_fig_name;
            MainWindow.selectedButton1 = null;
            MainWindow.selectedButton2 = null;
            if (Color == "white") MainWindow.last_move = 'b';
            else if (Color == "black") MainWindow.last_move = 'w';
            if (btn_fig_name != "empty")
            {
                if (btn_fig_name[0] == 'w') MainWindow.whiteFigures.Add((isTestMove ? MainWindow.beaten_figure_test : MainWindow.beaten_figure, btn_fig_name));
                else if (btn_fig_name[0] == 'b') MainWindow.blackFigures.Add((isTestMove ? MainWindow.beaten_figure_test : MainWindow.beaten_figure, btn_fig_name));
            }
            MainWindow.beaten_figure = null;
            MainWindow.beaten_figure_test = null;
            if (isPawn)
            {
                pawn.moves_enPassent = false;
                pawn.enPassentBtn = null;
            }
        }

        private void move_run(CustomData sb1Data, CustomData sb2Data)
        {
            PositionX = sb2Data.X;
            PositionY = sb2Data.Y;
            MainWindow.selectedButton2.Tag = sb2Data;

            if(this is Pawn pawn && pawn.moves_enPassent)
            {
                ((CustomData)pawn.enPassentBtn.Tag).figure = "empty";
            }
            ((CustomData)MainWindow.selectedButton2.Tag).figure = sb1Data.figure;
            ((CustomData)MainWindow.selectedButton1.Tag).figure = "empty";
        }
    }
}
