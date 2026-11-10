using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Collections;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using NSPawn;
using NSKing;
using NSQueen;
using NSRock;
using NSBishop;
using NSKnight;
using System.Windows.Threading;
using System.Text.RegularExpressions;


namespace ChessXpress
{
    internal class Check
    {
        public static bool checkIfCheck_init(int selfKing_positionX, int selfKing_positionY, string color)
        {
            string enemy_pawn;
            string enemy_king;
            string enemy_queen;
            string enemy_rock;
            string enemy_bishop;
            string enemy_knight;

            //Gegner Figuren deklarieren
            if (color == "white")
            {
                enemy_pawn = "bpawn";
                enemy_king = "bking";
                enemy_queen = "bqueen";
                enemy_rock = "brock";
                enemy_bishop = "bbishop";
                enemy_knight = "bknight";
            }
            else
            {
                enemy_pawn = "wpawn";
                enemy_king = "wking";
                enemy_queen = "wqueen";
                enemy_rock = "wrock";
                enemy_bishop = "wbishop";
                enemy_knight = "wknight";
            }

            if (checkIfCheck_QueenOrRock(selfKing_positionX, selfKing_positionY, enemy_queen, enemy_rock)
                && checkIfCheck_QueenOrBishop(selfKing_positionX, selfKing_positionY, enemy_queen, enemy_bishop)
                && checkIfCheck_pawn(selfKing_positionX, selfKing_positionY, enemy_pawn, color)
                && checkIfCheck_knight(selfKing_positionX, selfKing_positionY, enemy_knight)
                && checkIfCheck_king(selfKing_positionX, selfKing_positionY, enemy_king)) return false;
            return true;
        }

        private static bool checkIfCheck_QueenOrRock(int selfKing_positionX, int selfKing_positionY, string enemy_queen, string enemy_rock)
        {
            string figure;
            string button_tag;

            int i = selfKing_positionX - 1;
            int j = selfKing_positionY;
            //=>Nach links
            while (!(i < 0))
            {
                figure = ((CustomData)MainWindow.Buttons[j, i].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_queen || button_tag == enemy_rock) return false;
                else if (((CustomData)MainWindow.Buttons[j, i].Tag).figure != "empty") break;
                i--;
            }
            i = selfKing_positionX + 1;
            //=>Nach rechts
            while (!(i > 7))
            {
                figure = ((CustomData)MainWindow.Buttons[j, i].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_queen || button_tag == enemy_rock) return false;
                else if (((CustomData)MainWindow.Buttons[j, i].Tag).figure != "empty") break;
                i++;
            }
            i = selfKing_positionX;
            j = selfKing_positionY - 1;
            //=>Nach oben
            while (!(j < 0))
            {
                figure = ((CustomData)MainWindow.Buttons[j, i].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_queen || button_tag == enemy_rock) return false;
                else if (((CustomData)MainWindow.Buttons[j, i].Tag).figure != "empty") break;
                j--;
            }
            j = selfKing_positionY + 1;
            //=>Nach unten
            while (!(j > 7))
            {
                figure = ((CustomData)MainWindow.Buttons[j, i].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_queen || button_tag == enemy_rock) return false;
                else if (((CustomData)MainWindow.Buttons[j, i].Tag).figure != "empty") break;
                j++;
            }
            return true;
        }

        private static bool checkIfCheck_QueenOrBishop(int selfKing_positionX, int selfKing_positionY, string enemy_queen, string enemy_bishop)
        {
            string figure;
            string button_tag;
            //rechts unten
            int i = selfKing_positionX + 1;
            int j = selfKing_positionY + 1;
            while (!(i > 7 || j > 7))
            {
                figure = ((CustomData)MainWindow.Buttons[j, i].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_queen || button_tag == enemy_bishop) return false;
                else if (((CustomData)MainWindow.Buttons[j, i].Tag).figure != "empty") break;
                i++;
                j++;
            }

            //rechts oben
            i = selfKing_positionX + 1;
            j = selfKing_positionY - 1;
            while (!(i > 7 || j < 0))
            {
                figure = ((CustomData)MainWindow.Buttons[j, i].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_queen || button_tag == enemy_bishop) return false;
                else if (((CustomData)MainWindow.Buttons[j, i].Tag).figure != "empty") break;
                i++;
                j--;
            }

            //links unten
            i = selfKing_positionX - 1;
            j = selfKing_positionY + 1;
            while (!(i < 0 || j > 7))
            {
                figure = ((CustomData)MainWindow.Buttons[j, i].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_queen || button_tag == enemy_bishop) return false;
                else if (((CustomData)MainWindow.Buttons[j, i].Tag).figure != "empty") break;
                i--;
                j++;
            }

            //links oben
            i = selfKing_positionX - 1;
            j = selfKing_positionY - 1;
            while (!(i < 0 || j < 0))
            {
                figure = ((CustomData)MainWindow.Buttons[j, i].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_queen || button_tag == enemy_bishop) return false;
                else if (((CustomData)MainWindow.Buttons[j, i].Tag).figure != "empty") break;
                i--;
                j--;
            }
            return true;
        }

        private static bool checkIfCheck_pawn(int selfKing_positionX, int selfKing_positionY, string enemy_pawn, string color)
        {
            string figure;
            string button_tag;
            if (color == "white")
            {
                if (selfKing_positionY > 1)
                {
                    if (selfKing_positionX > 0)
                    {
                        figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX - 1].Tag).figure;
                        button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                        if (button_tag == enemy_pawn) return false;
                    }

                    if (selfKing_positionX < 7)
                    {
                        figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX + 1].Tag).figure;
                        button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                        if (button_tag == enemy_pawn) return false;
                    }
                }
            }
            else if (color == "black")
            {
                if (selfKing_positionY < 6)
                {
                    if (selfKing_positionX > 0)
                    {
                        figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX - 1].Tag).figure;
                        button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                        if (button_tag == enemy_pawn) return false;
                    }

                    if (selfKing_positionX < 7)
                    {
                        figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX + 1].Tag).figure;
                        button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                        if (button_tag == enemy_pawn) return false;
                    }
                }
            }
            return true;
        }

        private static bool checkIfCheck_knight(int selfKing_positionX, int selfKing_positionY, string enemy_knight)
        {
            string figure;
            string button_tag;
            //2 nach oben 1 nach links
            if (selfKing_positionX > 0 && selfKing_positionY > 1)
            {
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 2, selfKing_positionX - 1].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_knight) return false;
            }

            //2 nach oben 1 nach rechts
            if (selfKing_positionX < 7 && selfKing_positionY > 1)
            {
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 2, selfKing_positionX + 1].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_knight) return false;
            }

            //2 nach unten 1 nach links
            if (selfKing_positionX > 0 && selfKing_positionY < 6)
            {
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 2, selfKing_positionX - 1].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_knight) return false;
            }

            //2 nach unten 1 nach rechts
            if (selfKing_positionX < 7 && selfKing_positionY < 6)
            {
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 2, selfKing_positionX + 1].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_knight) return false;
            }

            //2 nach links 1 nach unten
            if (selfKing_positionX > 1 && selfKing_positionY < 7)
            {
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX - 2].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_knight) return false;
            }

            //2 nach links 1 nach oben
            if (selfKing_positionX > 1 && selfKing_positionY > 0)
            {
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX - 2].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_knight) return false;
            }

            //2 nach rechts 1 nach unten
            if (selfKing_positionX < 6 && selfKing_positionY < 7)
            {
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX + 2].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_knight) return false;
            }

            //2 nach rechts 1 nach oben
            if (selfKing_positionX < 6 && selfKing_positionY > 0)
            {
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX + 2].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_knight) return false;
            }
            return true;
        }

        private static bool checkIfCheck_king(int selfKing_positionX, int selfKing_positionY, string enemy_king)
        {
            string figure;
            string button_tag;
            //Alles über dem König
            if (selfKing_positionY > 0)
            {
                //gerade nach oben
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_king) return false;

                //oben links
                if (selfKing_positionX > 0)
                {
                    figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX - 1].Tag).figure;
                    button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                    if (button_tag == enemy_king) return false;
                }
                //oben rechts
                if (selfKing_positionX < 7)
                {
                    figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX + 1].Tag).figure;
                    button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                    if (button_tag == enemy_king) return false;
                }
            }

            //Alles rechts vom König
            if (selfKing_positionX > 7)
            {
                //gerade nach rechts
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY, selfKing_positionX + 1].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_king) return false;

                //rechts oben
                if (selfKing_positionY > 0)
                {
                    figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX + 1].Tag).figure;
                    button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                    if (button_tag == enemy_king) return false;
                }
                //rechts unten
                if (selfKing_positionY < 7)
                {
                    figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX + 1].Tag).figure;
                    button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                    if (button_tag == enemy_king) return false;
                }
            }

            //Alles unter dem König
            if (selfKing_positionY < 7)
            {
                //gerade nach unten
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_king) return false;

                //unten links
                if (selfKing_positionX > 0)
                {
                    figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX - 1].Tag).figure;
                    button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                    if (button_tag == enemy_king) return false;
                }
                //unten rechts
                if (selfKing_positionX < 7)
                {
                    figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX + 1].Tag).figure;
                    button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                    if (button_tag == enemy_king) return false;
                }
            }

            //Alles links vom König
            if (selfKing_positionX > 0)
            {
                //gerade nach links
                figure = ((CustomData)MainWindow.Buttons[selfKing_positionY, selfKing_positionX - 1].Tag).figure;
                button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                if (button_tag == enemy_king) return false;

                //links oben
                if (selfKing_positionY > 0)
                {
                    figure = ((CustomData)MainWindow.Buttons[selfKing_positionY - 1, selfKing_positionX - 1].Tag).figure;
                    button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                    if (button_tag == enemy_king) return false;
                }
                //links unten
                if (selfKing_positionY < 7)
                {
                    figure = ((CustomData)MainWindow.Buttons[selfKing_positionY + 1, selfKing_positionX - 1].Tag).figure;
                    button_tag = Regex.Replace(figure, @"[\d]", string.Empty);
                    if (button_tag == enemy_king) return false;
                }
            }
            return true;
        }
    }
}
