using NSBishop;
using NSFigure;
using NSKnight;
using NSPawn;
using NSQueen;
using NSRock;
using NSKing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Net.NetworkInformation;
using System.Windows.Documents;

namespace ChessXpress
{
    internal class CheckMate
    {
        private static List<(Object, string)> Figures;
        private static int pawn_move1;
        private static int pawn_move2;

        public static bool checkIfMate_init(King enemyKing, King selfking)
        {
            if (enemyKing.Color == "white")
            {
                Figures = MainWindow.whiteFigures;
                pawn_move1 = -1;
                pawn_move2 = -2;
            }
            else if (enemyKing.Color == "black")
            {
                Figures = MainWindow.blackFigures;
                pawn_move1 = 1;
                pawn_move2 = 2;
            }

            foreach ((object figure, string _) in Figures)
            {
                if(figure is Queen queen)
                {
                    if(checkIfMate_queen(queen, enemyKing)) return false;
                }
                else if(figure is Rock rock)
                {
                    if(checkIfMate_rock(rock, enemyKing)) return false;
                }
                else if (figure is Pawn pawn)
                {
                    if(checkIfMate_pawn(pawn, enemyKing)) return false;
                }
                else if(figure is Bishop bishop)
                {
                    if(checkIfMate_bishop(bishop, enemyKing)) return false;
                }
                else if(figure is Knight knight)
                {
                    if (checkIfMate_knight(knight, enemyKing)) return false;
                }
                else if(figure is King king)
                {
                    if (checkIfMate_king(king, enemyKing)) return false;
                }
            }
            if (!enemyKing.isCheck) MainWindow.stalemate = true;
            return true;
        }

        private static bool checkIfMate_queen(Queen queen, King enemyKing)
        {

            //Straight nach oben (X; Y--)
            for (int r = queen.PositionY - 1; r >= 0; r--)
            {
                createButtons(queen);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[r, queen.PositionX].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, queen, enemyKing)) return true;
            }

            //Straight nach unten (X; Y++)
            for (int r = queen.PositionY + 1; r < 8; r++)
            {
                createButtons(queen);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[r, queen.PositionX].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, queen, enemyKing)) return true;
            }

            //Straight nach links (X--; Y)
            for (int c = queen.PositionX - 1; c >= 0; c--)
            {
                createButtons(queen);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[queen.PositionY, c].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, queen, enemyKing)) return true;
            }

            //Straight nach rechts (X++; Y)
            for (int c = queen.PositionX + 1; c < 8; c++)
            {
                createButtons(queen);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[queen.PositionY, c].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, queen, enemyKing)) return true;
            }

            //Nach oben links (X--; Y--)
            int i = queen.PositionY - 1;
            int j = queen.PositionX - 1;
            while (i >= 0 && j >= 0)
            {
                createButtons(queen);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[i, j].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, queen, enemyKing)) return true;
                i--;
                j--;
            }

            //Nach unten links (X--; Y++)
            i = queen.PositionY + 1;
            j = queen.PositionX - 1;
            while (i < 8 && j >= 0)
            {
                createButtons(queen);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[i, j].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, queen, enemyKing)) return true;
                i++;
                j--;
            }

            //Nach oben rechts (X++; Y--)
            i = queen.PositionY - 1;
            j = queen.PositionX + 1;
            while (i >= 0 && j < 8)
            {
                createButtons(queen);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[i, j].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, queen, enemyKing)) return true;
                i--;
                j++;
            }

            //Nach unten rechts (X++; Y++)
            i = queen.PositionY + 1;
            j = queen.PositionX + 1;
            while (i < 8 && j < 8)
            {
                createButtons(queen);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[i, j].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, queen, enemyKing)) return true;
                i++;
                j++;
            }
            return false;
        }

        private static bool checkIfMate_rock(Rock rock, King enemyKing)
        {
            //Straight nach oben (X; Y--)
            for (int r = rock.PositionY - 1; r >= 0; r--)
            {
                createButtons(rock);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[r, rock.PositionX].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, rock, enemyKing)) return true;
            }

            //Straight nach unten (X; Y++)
            for (int r = rock.PositionY + 1; r < 8; r++)
            {
                createButtons(rock);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[r, rock.PositionX].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, rock, enemyKing)) return true;
            }

            //Straight nach links (X--; Y)
            for (int c =rock.PositionX - 1; c >= 0; c--)
            {
                createButtons(rock);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[rock.PositionY, c].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, rock, enemyKing)) return true;
            }

            //Straight nach rechts (X++; Y)
            for (int c = rock.PositionX + 1; c < 8; c++)
            {
                createButtons(rock);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[rock.PositionY, c].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, rock, enemyKing)) return true;
            }
            return false;
        }

        private static bool checkIfMate_pawn(Pawn pawn, King enemyKing)
        {
            createButtons(pawn);
            if ((pawn.PositionY > 0 && pawn.Color == "white") || (pawn.PositionY < 7 && pawn.Color == "black"))
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[pawn.PositionY + pawn_move1, pawn.PositionX].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, pawn, enemyKing)) return true;
            }

            createButtons(pawn);
            if ((pawn.PositionY > 0 && pawn.Color == "white" && pawn.PositionX > 0) || (pawn.PositionY < 7 && pawn.Color == "black" && pawn.PositionX > 0))
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[pawn.PositionY + pawn_move1, pawn.PositionX - 1].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, pawn, enemyKing)) return true;
            }

            createButtons(pawn);
            if ((pawn.PositionY > 0 && pawn.Color == "white" && pawn.PositionX < 7) || (pawn.PositionY < 7 && pawn.Color == "black" && pawn.PositionX < 7))
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[pawn.PositionY + pawn_move1, pawn.PositionX + 1].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, pawn, enemyKing)) return true;
            }

            createButtons(pawn);
            if ((pawn.PositionY == 6 && pawn.Color == "white") || pawn.PositionY == 1 && pawn.Color == "black") MainWindow.selectedButton2.Tag = MainWindow.Buttons[pawn.PositionY + pawn_move2, pawn.PositionX].Tag;
            else return false;
            if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, pawn, enemyKing)) return true;
            return false;
        }

        private static bool checkIfMate_bishop(Bishop bishop, King enemyKing)
        {
            //Nach oben links (X--; Y--)
            int i = bishop.PositionY - 1;
            int j = bishop.PositionX - 1;
            while (i >= 0 && j >= 0)
            {
                createButtons(bishop);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[i, j].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, bishop, enemyKing)) return true;
                i--;
                j--;
            }

            //Nach unten links (X--; Y++)
            i = bishop.PositionY + 1;
            j = bishop.PositionX - 1;
            while (i < 8 && j >= 0)
            {
                createButtons(bishop);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[i, j].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, bishop, enemyKing)) return true;
                i++;
                j--;
            }

            //Nach oben rechts (X++; Y--)
            i = bishop.PositionY - 1;
            j = bishop.PositionX + 1;
            while (i >= 0 && j < 8)
            {
                createButtons(bishop);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[i, j].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, bishop, enemyKing)) return true;
                i--;
                j++;
            }

            //Nach unten rechts (X++; Y++)
            i = bishop.PositionY + 1;
            j = bishop.PositionX + 1;
            while (i < 8 && j < 8)
            {
                createButtons(bishop);
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[i, j].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, bishop, enemyKing)) return true;
                i++;
                j++;
            }
            return false;
        }

        private static bool checkIfMate_knight(Knight knight, King enemyKing)
        {

            //2 hoch 1 links (X--; Y-2)
            createButtons(knight);
            if (knight.PositionY - 2 >= 0 && knight.PositionX - 1 >= 0)
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[knight.PositionY - 2, knight.PositionX - 1].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, knight, enemyKing)) return true;
            }
            //2 hoch 1 rechts (X++; Y-2)
            createButtons(knight);
            if (knight.PositionY - 2 >= 0 && knight.PositionX + 1 < 8)
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[knight.PositionY - 2, knight.PositionX + 1].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, knight, enemyKing)) return true;
            }
            //2 rechts 1 hoch (X+2; Y--)
            createButtons(knight);
            if (knight.PositionY - 1 >= 0 && knight.PositionX + 2 < 8)
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[knight.PositionY - 1, knight.PositionX + 2].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, knight, enemyKing)) return true;
            }
            //2 rechts 1 runter (X+2; Y++)
            createButtons(knight);
            if (knight.PositionY + 1 < 8 && knight.PositionX + 2 < 8)
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[knight.PositionY + 1, knight.PositionX + 2].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, knight, enemyKing)) return true;
            }
            //2 runter 1 rechts (X++; Y+2)
            createButtons(knight);
            if (knight.PositionY + 2 < 8 && knight.PositionX + 1 < 8)
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[knight.PositionY + 2, knight.PositionX + 1].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, knight, enemyKing)) return true;
            }
            //2 runter 1 links (X--; Y+2)
            createButtons(knight);
            if (knight.PositionY + 2 < 8 && knight.PositionX - 1 >= 0)
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[knight.PositionY + 2, knight.PositionX - 1].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, knight, enemyKing)) return true;
            }
            //2 links 1 runter (X-2; Y++)
            createButtons(knight);
            if (knight.PositionY + 1 < 8 && knight.PositionX - 2 >= 0)
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[knight.PositionY + 1, knight.PositionX - 2].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, knight, enemyKing)) return true;
            }
            //2 links 1 hoch (X-2; Y--)
            createButtons(knight);
            if (knight.PositionY - 1 >= 0 && knight.PositionX - 2 >= 0)
            {
                MainWindow.selectedButton2.Tag = MainWindow.Buttons[knight.PositionY - 1, knight.PositionX - 2].Tag;
                if (move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, knight, enemyKing)) return true;
            }
            return false;
        }
        private static bool checkIfMate_king(King king, King enemyKing)
        {
            ((CustomData)MainWindow.Buttons[7, 2].Tag).figure = ((CustomData)MainWindow.Buttons[7, 2].Tag).figure;
            if (checkIfMate_kingLoop(-1, 1, king, enemyKing)
                || checkIfMate_kingLoop(-1, 0, king, enemyKing)
                || checkIfMate_kingLoop(-1, -1, king, enemyKing)
                || checkIfMate_kingLoop(0, -1, king, enemyKing)
                || checkIfMate_kingLoop(1, -1, king, enemyKing)
                || checkIfMate_kingLoop(1, 0, king, enemyKing)
                || checkIfMate_kingLoop(1, 1, king, enemyKing)
                || checkIfMate_kingLoop(0, 1, king, enemyKing)) return true;
            return false;
        }

        private static bool checkIfMate_kingLoop(int targetX, int targetY, King king, King enemyKing)
        {
            if (!(king.PositionX + targetX < 8 && king.PositionX + targetX >= 0
                && king.PositionY + targetY < 8 && king.PositionY + targetY >= 0)) return false;
            createButtons(king);
            MainWindow.selectedButton2.Tag = MainWindow.Buttons[king.PositionY + targetY, king.PositionX + targetX].Tag;
            //Hat man 2x seine eigene Farbe geklickt?
            return move((CustomData)MainWindow.selectedButton1.Tag, (CustomData)MainWindow.selectedButton2.Tag, king, enemyKing);
        }

        private static void createButtons(object obj)
        {
            if(obj is Chessfigure figure)
            {
                MainWindow.selectedButton1 = new Button();
                MainWindow.selectedButton2 = new Button();
                MainWindow.selectedButton1.Tag = MainWindow.Buttons[figure.PositionY, figure.PositionX].Tag;
            }
        }

        private static bool move(CustomData sb1Data, CustomData sb2Data, Chessfigure self, King enemyKing)
        {
            if (sb2Data.figure[0] == sb1Data.figure[0]) return false;
            if (self.move(enemyKing, true))
            {
                self.resetMove(sb1Data, sb2Data, true);
                return true;
            }
            return false;
        }
    }
}
