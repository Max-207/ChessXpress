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
using NSFigure;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChessXpress
{
    public class CustomData
    {
        public string figure { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class TableEntry
    {
        public string Move { get; set; }
        public TableEntry(string move)
        {
            Move = move;
        }
    }

    public partial class MainWindow : Window
    {
        public ObservableCollection<TableEntry> whiteMoveEntries { get; set; }
        public ObservableCollection<TableEntry> blackMoveEntries { get; set; }
        private TableEntry currentEntry = null;

        public static Button[,] Buttons;
        public static Button selectedButton1;
        public static Button selectedButton2;
        public static char last_move = 'n';
        public static Timer w_timer = new Timer(1000);
        public static Timer b_timer = new Timer(1000);
        public static int countdownWhite;
        public static int countdownBlack;
        public static bool pause = true;
        public static List<(Object, string)> whiteFigures = new List<(Object, string)>();
        public static List<Object> whiteFigures_new = new List<Object>();
        public static List<(Object, string)> blackFigures = new List<(Object, string)>();
        public static List<Object> blackFigures_new = new List<Object>();
        public static object beaten_figure;
        public static object beaten_figure_test;
        public static bool stalemate = false;
        public static CustomData changePawn_Data;
        public static int positionNumber = 1;
        public static int positionsCount = 1;

        public static Image wpawnIMG = new Image { Source = new BitmapImage(new Uri("img/figures/w-pawn.png", UriKind.Relative)) };
        public static Image wkingIMG = new Image { Source = new BitmapImage(new Uri("img/figures/w-king.png", UriKind.Relative)) };
        public static Image wqueenIMG = new Image { Source = new BitmapImage(new Uri("img/figures/w-queen.png", UriKind.Relative)) };
        public static Image wrockIMG = new Image { Source = new BitmapImage(new Uri("img/figures/w-rock.png", UriKind.Relative)) };
        public static Image wbishopIMG = new Image { Source = new BitmapImage(new Uri("img/figures/w-bishop.png", UriKind.Relative)) };
        public static Image wknightIMG = new Image { Source = new BitmapImage(new Uri("img/figures/w-knight.png", UriKind.Relative)) };
        public static Image bpawnIMG = new Image { Source = new BitmapImage(new Uri("img/figures/b-pawn.png", UriKind.Relative)) };
        public static Image bkingIMG = new Image { Source = new BitmapImage(new Uri("img/figures/b-king.png", UriKind.Relative)) };
        public static Image bqueenIMG = new Image { Source = new BitmapImage(new Uri("img/figures/b-queen.png", UriKind.Relative)) };
        public static Image brockIMG = new Image { Source = new BitmapImage(new Uri("img/figures/b-rock.png", UriKind.Relative)) };
        public static Image bbishopIMG = new Image { Source = new BitmapImage(new Uri("img/figures/b-bishop.png", UriKind.Relative)) };
        public static Image bknightIMG = new Image { Source = new BitmapImage(new Uri("img/figures/b-knight.png", UriKind.Relative)) };

        Pawn wpawn1;
        Pawn wpawn2;
        Pawn wpawn3;
        Pawn wpawn4;
        Pawn wpawn5;
        Pawn wpawn6;
        Pawn wpawn7;
        Pawn wpawn8;
        King wking1;
        Queen wqueen1;
        Rock wrock1;
        Rock wrock2;
        Bishop wbishop1;
        Bishop wbishop2;
        Knight wknight1;
        Knight wknight2;

        Pawn bpawn1;
        Pawn bpawn2;
        Pawn bpawn3;
        Pawn bpawn4;
        Pawn bpawn5;
        Pawn bpawn6;
        Pawn bpawn7;
        Pawn bpawn8;
        King bking1;
        Queen bqueen1;
        Rock brock1;
        Rock brock2;
        Bishop bbishop1;
        Bishop bbishop2;
        Knight bknight1;
        Knight bknight2;

        public MainWindow()
        {
            InitializeComponent();
            whiteMoveEntries = new ObservableCollection<TableEntry>();
            whiteMovesTable.ItemsSource = whiteMoveEntries;
            blackMoveEntries = new ObservableCollection<TableEntry>();
            blackMovesTable.ItemsSource = blackMoveEntries;
            initFigures();
            createButtons();
        }

        private void initFigures()
        {
            wpawn1 = new Pawn("wpawn1", "white", 0, 6, wpawnIMG.Source);
            wpawn2 = new Pawn("wpawn2", "white", 1, 6, wpawnIMG.Source);
            wpawn3 = new Pawn("wpawn3", "white", 2, 6, wpawnIMG.Source);
            wpawn4 = new Pawn("wpawn4", "white", 3, 6, wpawnIMG.Source);
            wpawn5 = new Pawn("wpawn5", "white", 4, 6, wpawnIMG.Source);
            wpawn6 = new Pawn("wpawn6", "white", 5, 6, wpawnIMG.Source);
            wpawn7 = new Pawn("wpawn7", "white", 6, 6, wpawnIMG.Source);
            wpawn8 = new Pawn("wpawn8", "white", 7, 6, wpawnIMG.Source);
            wking1 = new King("wking1", "white", 4, 7, wkingIMG.Source);
            wqueen1 = new Queen("wqueen1", "white", 3, 7, wqueenIMG.Source);
            wrock1 = new Rock("wrock1", "white", 0, 7, wrockIMG.Source);
            wrock2 = new Rock("wrock2", "white", 7, 7, wrockIMG.Source);
            wbishop1 = new Bishop("wbishop1", "white", 2, 7, wbishopIMG.Source);
            wbishop2 = new Bishop("wbishop2", "white", 5, 7, wbishopIMG.Source);
            wknight1 = new Knight("wknight1", "white", 1, 7, wknightIMG.Source);
            wknight2 = new Knight("wknight2", "white", 6, 7, wknightIMG.Source);

            bpawn1 = new Pawn("bpawn1", "black", 0, 1, bpawnIMG.Source);
            bpawn2 = new Pawn("bpawn2", "black", 1, 1, bpawnIMG.Source);
            bpawn3 = new Pawn("bpawn3", "black", 2, 1, bpawnIMG.Source);
            bpawn4 = new Pawn("bpawn4", "black", 3, 1, bpawnIMG.Source);
            bpawn5 = new Pawn("bpawn5", "black", 4, 1, bpawnIMG.Source);
            bpawn6 = new Pawn("bpawn6", "black", 5, 1, bpawnIMG.Source);
            bpawn7 = new Pawn("bpawn7", "black", 6, 1, bpawnIMG.Source);
            bpawn8 = new Pawn("bpawn8", "black", 7, 1, bpawnIMG.Source);
            bking1 = new King("bking1", "black", 4, 0, bkingIMG.Source);
            bqueen1 = new Queen("bqueen1", "black", 3, 0, bqueenIMG.Source);
            brock1 = new Rock("brock1", "black", 0, 0, brockIMG.Source);
            brock2 = new Rock("brock2", "black", 7, 0, brockIMG.Source);
            bbishop1 = new Bishop("bbishop1", "black", 2, 0, bbishopIMG.Source);
            bbishop2 = new Bishop("bbishop2", "black", 5, 0, bbishopIMG.Source);
            bknight1 = new Knight("bknight1", "black", 1, 0, bknightIMG.Source);
            bknight2 = new Knight("bnight2", "black", 6, 0, bknightIMG.Source);

            addFigures();
        }
        private void addFigures()
        {
            whiteFigures.Add((wpawn1, "wpawn1"));
            whiteFigures.Add((wpawn2, "wpawn2"));
            whiteFigures.Add((wpawn3, "wpawn3"));
            whiteFigures.Add((wpawn4, "wpawn4"));
            whiteFigures.Add((wpawn5, "wpawn5"));
            whiteFigures.Add((wpawn6, "wpawn6"));
            whiteFigures.Add((wpawn7, "wpawn7"));
            whiteFigures.Add((wpawn8, "wpawn8"));
            whiteFigures.Add((wking1, "wking1"));
            whiteFigures.Add((wqueen1, "wqueen1"));
            whiteFigures.Add((wrock1, "wrock1"));
            whiteFigures.Add((wrock2, "wrock2"));
            whiteFigures.Add((wbishop1, "wbishop1"));
            whiteFigures.Add((wbishop2, "wbishop2"));
            whiteFigures.Add((wknight1, "wknight1"));
            whiteFigures.Add((wknight2, "wknight2"));

            blackFigures.Add((bpawn1, "bpawn1"));
            blackFigures.Add((bpawn2, "bpawn2"));
            blackFigures.Add((bpawn3, "bpawn3"));
            blackFigures.Add((bpawn4, "bpawn4"));
            blackFigures.Add((bpawn5, "bpawn5"));
            blackFigures.Add((bpawn6, "bpawn6"));
            blackFigures.Add((bpawn7, "bpawn7"));
            blackFigures.Add((bpawn8, "bpawn8"));
            blackFigures.Add((bking1, "bking1"));
            blackFigures.Add((bqueen1, "bqueen1"));
            blackFigures.Add((brock1, "brock1"));
            blackFigures.Add((brock2, "brock2"));
            blackFigures.Add((bbishop1, "bbishop1"));
            blackFigures.Add((bbishop2, "bbishop2"));
            blackFigures.Add((bknight1, "bknight1"));
            blackFigures.Add((bknight2, "bknight2"));
        }
        private void createButtons()
        {
            Buttons = new Button[8, 8];
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Buttons[r, c] = new Button();
                    Buttons[r, c].Click += move;
                    Buttons[r, c].Cursor = Cursors.Hand;
                    Buttons[r, c].BorderThickness = new Thickness(0);
                    Chessboard.Children.Add(Buttons[r, c]);
                    Grid.SetRow(Buttons[r, c], r);
                    Grid.SetColumn(Buttons[r, c], c);
                    Buttons[r, c].Tag = new CustomData { figure = "empty", X = c, Y = r };
                    Buttons[r, c].IsHitTestVisible = false;

                    if (r == 0 || r == 2 || r == 4 || r == 6)
                    {
                        if (c == 0 || c == 2 || c == 4 || c == 6) Buttons[r, c].Background = new SolidColorBrush(Colors.White);
                        else Buttons[r, c].Background = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        if (c == 1 || c == 3 || c == 5 || c == 7) Buttons[r, c].Background = new SolidColorBrush(Colors.White);
                        else Buttons[r, c].Background = new SolidColorBrush(Colors.Black);
                    }
                }
            }
            TimerMenu.Visibility = Visibility.Collapsed;
        }

        private void createFigures()
        {
            Buttons[6, 0].Content = new Image { Source = wpawn1.Image };
            ((CustomData)Buttons[6, 0].Tag).figure = "wpawn1";
            Buttons[6, 1].Content = new Image { Source = wpawn2.Image };
            ((CustomData)Buttons[6, 1].Tag).figure = "wpawn2";
            Buttons[6, 2].Content = new Image { Source = wpawn3.Image };
            ((CustomData)Buttons[6, 2].Tag).figure = "wpawn3";
            Buttons[6, 3].Content = new Image { Source = wpawn4.Image };
            ((CustomData)Buttons[6, 3].Tag).figure = "wpawn4";
            Buttons[6, 4].Content = new Image { Source = wpawn5.Image };
            ((CustomData)Buttons[6, 4].Tag).figure = "wpawn5";
            Buttons[6, 5].Content = new Image { Source = wpawn6.Image };
            ((CustomData)Buttons[6, 5].Tag).figure = "wpawn6";
            Buttons[6, 6].Content = new Image { Source = wpawn7.Image };
            ((CustomData)Buttons[6, 6].Tag).figure = "wpawn7";
            Buttons[6, 7].Content = new Image { Source = wpawn8.Image };
            ((CustomData)Buttons[6, 7].Tag).figure = "wpawn8";
            Buttons[7, 4].Content = new Image { Source = wking1.Image };
            ((CustomData)Buttons[7, 4].Tag).figure = "wking1";
            Buttons[7, 3].Content = new Image { Source = wqueen1.Image };
            ((CustomData)Buttons[7, 3].Tag).figure = "wqueen1";
            Buttons[7, 0].Content = new Image { Source = wrock1.Image };
            ((CustomData)Buttons[7, 0].Tag).figure = "wrock1";
            wking1.CastlingButtons.Add((Buttons[7, 0], wrock1));
            Buttons[7, 7].Content = new Image { Source = wrock2.Image };
            ((CustomData)Buttons[7, 7].Tag).figure = "wrock2";
            wking1.CastlingButtons.Add((Buttons[7, 7], wrock2));
            Buttons[7, 2].Content = new Image { Source = wbishop1.Image };
            ((CustomData)Buttons[7, 2].Tag).figure = "wbishop1";
            Buttons[7, 5].Content = new Image { Source = wbishop2.Image };
            ((CustomData)Buttons[7, 5].Tag).figure = "wbishop2";
            Buttons[7, 1].Content = new Image { Source = wknight1.Image };
            ((CustomData)Buttons[7, 1].Tag).figure = "wknight1";
            Buttons[7, 6].Content = new Image { Source = wknight2.Image };
            ((CustomData)Buttons[7, 6].Tag).figure = "wknight2";

            Buttons[1, 0].Content = new Image { Source = bpawn1.Image };
            ((CustomData)Buttons[1, 0].Tag).figure = "bpawn1";
            Buttons[1, 1].Content = new Image { Source = bpawn2.Image };
            ((CustomData)Buttons[1, 1].Tag).figure = "bpawn2";
            Buttons[1, 2].Content = new Image { Source = bpawn3.Image };
            ((CustomData)Buttons[1, 2].Tag).figure = "bpawn3";
            Buttons[1, 3].Content = new Image { Source = bpawn4.Image };
            ((CustomData)Buttons[1, 3].Tag).figure = "bpawn4";
            Buttons[1, 4].Content = new Image { Source = bpawn5.Image };
            ((CustomData)Buttons[1, 4].Tag).figure = "bpawn5";
            Buttons[1, 5].Content = new Image { Source = bpawn6.Image };
            ((CustomData)Buttons[1, 5].Tag).figure = "bpawn6";
            Buttons[1, 6].Content = new Image { Source = bpawn7.Image };
            ((CustomData)Buttons[1, 6].Tag).figure = "bpawn7";
            Buttons[1, 7].Content = new Image { Source = bpawn8.Image };
            ((CustomData)Buttons[1, 7].Tag).figure = "bpawn8";
            Buttons[0, 4].Content = new Image { Source = bking1.Image };
            ((CustomData)Buttons[0, 4].Tag).figure = "bking1";
            Buttons[0, 3].Content = new Image { Source = bqueen1.Image };
            ((CustomData)Buttons[0, 3].Tag).figure = "bqueen1";
            Buttons[0, 0].Content = new Image { Source = brock1.Image };
            ((CustomData)Buttons[0, 0].Tag).figure = "brock1";
            bking1.CastlingButtons.Add((Buttons[0, 0], brock1));
            Buttons[0, 7].Content = new Image { Source = brock2.Image };
            ((CustomData)Buttons[0, 7].Tag).figure = "brock2";
            bking1.CastlingButtons.Add((Buttons[0, 7], brock2));
            Buttons[0, 2].Content = new Image { Source = bbishop1.Image };
            ((CustomData)Buttons[0, 2].Tag).figure = "bbishop1";
            Buttons[0, 5].Content = new Image { Source = bbishop2.Image };
            ((CustomData)Buttons[0, 5].Tag).figure = "bbishop2";
            Buttons[0, 1].Content = new Image { Source = bknight1.Image };
            ((CustomData)Buttons[0, 1].Tag).figure = "bknight1";
            Buttons[0, 6].Content = new Image { Source = bknight2.Image };
            ((CustomData)Buttons[0, 6].Tag).figure = "bknight2";
        }

        private void Slider_MinuteChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelMinutes.Content = (int)sliderMinutes.Value + " Minuten";
        }

        private void Slider_SecondChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelSeconds.Content = (int)sliderSeconds.Value + " Sekunden";
        }

        private void init_Time_Click(object sender, RoutedEventArgs e)
        {
            startInteractButton.IsHitTestVisible = true;
            restartButton.Width = 100;
            if (sliderMinutes.Value == 0 && sliderSeconds.Value < 30)
            {
                return;
            }

            //start game with time properties
            for (int r = 0; r < 8; r++) for (int c = 0; c < 8; c++) Buttons[r, c].IsHitTestVisible = true;
            createFigures();
            BoardPositions.safePosition();
            blackText.Text = whiteText.Text = ((int)sliderMinutes.Value < 10 ? "0" : "") + (int)sliderMinutes.Value + ":";
            blackText.Text = whiteText.Text += ((int)sliderSeconds.Value < 10 ? "0" : "") + (int)sliderSeconds.Value;
            startInteractButton.Visibility = Visibility.Visible;
            StartButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Collapsed;
            TimerStartMenu.Visibility = Visibility.Collapsed;
            TimerMenu.Visibility = Visibility.Visible;
            whiteText.FontSize = 50;
            blackText.FontSize = 50;
            countdownWhite = countdownBlack = (int)sliderMinutes.Value * 60 + (int)sliderSeconds.Value;
            w_timer.Elapsed += letWhiteTick;
            b_timer.Elapsed += letBlackTick;
        }

        private void startTimer_Click(object sender, RoutedEventArgs e)
        {
            for (int r = 0; r < 8; r++) for (int c = 0; c < 8; c++) Buttons[r, c].IsHitTestVisible = true;
            sliderMinutes.Value = 0;
            sliderSeconds.Value = 0;
            StartButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Visible;
            startInteractButton.Click -= startTimer_Click;
            startInteractButton.Click += stopTimer_Click;
            (last_move == 'w' ? b_timer : (last_move == 'b' || last_move == 'n' ? w_timer : null)).Start();
            pause = false;
        }

        private void stopTimer_Click(object sender, RoutedEventArgs e)
        {
            for(int r = 0; r < 8; r++) for(int c = 0; c < 8; c++) Buttons[r, c].IsHitTestVisible = false;
            StartButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Collapsed;
            startInteractButton.Click -= stopTimer_Click;
            startInteractButton.Click += startTimer_Click;
            w_timer.Stop();
            b_timer.Stop();
            pause = true;
        }

        private void letWhiteTick(object sender, EventArgs e)
        {
            countdownWhite--;

            int minutes = countdownWhite / 60;
            int seconds = countdownWhite % 60;

            Dispatcher.Invoke(() =>
            {
                whiteText.Text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
            });

            if (countdownWhite <= 0)
            {
                w_timer.Stop();
                w_timer = null;
                b_timer = null;
            }
        }

        private void letBlackTick(object sender, EventArgs e)
        {
            countdownBlack--;

            int minutes = countdownBlack / 60; // Minuten berechnen
            int seconds = countdownBlack % 60; // Sekunden berechnen

            Dispatcher.Invoke(() =>
            {
                blackText.Text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
            });

            if (countdownBlack <= 0)
            {
                b_timer.Stop();
                w_timer = null;
                b_timer = null;
            }
        }

        public void move(object sender, EventArgs e)
        {
            if (pause || !checkButton(sender, e)) return;
            CustomData senderData = (CustomData)((Button)sender).Tag;
            char fig_col = ((CustomData)selectedButton1.Tag).figure[0];
            if (fig_col == 'w') checkFigure(senderData, bking1, 0, b_timer, w_timer);
            else if (fig_col == 'b') checkFigure(senderData, wking1, 7, w_timer, b_timer);
        }

        private bool checkButton(object sender, EventArgs e)
        {

            //Wurde 2x das selbe Feld geklickt?
            if (sender as Button == selectedButton1) return false;

            //Wurde ins leere geklickt ohne Vorauswahl?
            if (selectedButton1 == null)
            {
                //Soll damit etwas ausgewählt werden ist es ein "leerer Klick" ?
                if ((sender as Button).Content == null) return false;
                selectedButton1 = sender as Button;
                return false;
            }

            CustomData sb1Data = (CustomData)selectedButton1.Tag;
            CustomData sb2Data = (CustomData)(sender as Button).Tag;
            //Wurde Rochade ausgewählt?
            if (!(sb1Data.figure[0] == sb2Data.figure[0] && sb1Data.figure.Contains("king")
                &&((sb2Data.figure.Contains("rock")
                ||(sb2Data.X == 1 && ((CustomData)Buttons[sb2Data.Y, sb2Data.X - 1].Tag).figure.Contains("rock"))
                || (sb2Data.X == 2 && ((CustomData)Buttons[sb2Data.Y, sb2Data.X - 2].Tag).figure.Contains("rock"))
                || (sb2Data.X == 6 && ((CustomData)Buttons[sb2Data.Y, sb2Data.X + 1].Tag).figure.Contains("rock"))))))
            {
                if ((w_timer.Enabled && sb1Data.figure[0] == 'b' && sb2Data.figure[0] == 'w')
                    || (b_timer.Enabled && sb1Data.figure[0] == 'w' && sb2Data.figure[0] == 'b'))
                {
                    selectedButton1 = null;
                    selectedButton2 = null;
                    return false;
                }

                //Hat man 2x seine eigene Farbe geklickt?
                if (!((CustomData)(sender as Button).Tag).figure.Contains("rock")
                    || !((CustomData)selectedButton1.Tag).figure.Contains("wking1")
                    || ((CustomData)(sender as Button).Tag).figure[0] != ((CustomData)selectedButton1.Tag).figure[0])
                {
                    if (((CustomData)(sender as Button).Tag).figure[0] == ((CustomData)selectedButton1.Tag).figure[0])
                    {
                        selectedButton1 = sender as Button;
                        return false;
                    }
                }
            }
            //Es bleibt nur noch übrig: Auswahlklick, Zug oder Schlagzug
            selectedButton2 = sender as Button;
            return true;
        }

        private void checkFigure(CustomData senderData, King enemyKing, int lastRow,Timer timer_toStart, Timer timer_toStop)
        {
            King selfKing = enemyKing.Color == "white" ? bking1 : wking1;
            CustomData sb1Tag = (CustomData)selectedButton1.Tag;
            CustomData sb1Data = new CustomData { figure = sb1Tag.figure, X = sb1Tag.X, Y = sb1Tag.Y };
            CustomData sb2Tag = (CustomData)selectedButton2.Tag;
            CustomData sb2Data = new CustomData { figure = sb2Tag.figure, X = sb2Tag.X, Y = sb2Tag.Y };
            string figure_name = sb1Data.figure;
            if (!checkMove(figure_name, selfKing)) return;
            selfKing.isCheck = false;
            
            wking1.moved = sb1Data.figure == "wking1" ? true : wking1.moved;
            bking1.moved = sb1Data.figure == "bking1" ? true : bking1.moved;
            
            if (senderData.Y == lastRow && sb1Data.figure.Contains("pawn")) changePawn(senderData, figure_name[0]);
            else
            {
                enemyKing.isCheck = checkIfCheck(enemyKing.Color);
                int posx, posy;
                posx = !sb1Data.figure.Contains("king") ? selfKing.PositionX : sb1Data.X;
                posy = !sb1Data.figure.Contains("king") ? selfKing.PositionY : sb1Data.Y;
                changeColor(selfKing, enemyKing, posx, posy);
                timer_toStop.Stop();
                timer_toStart.Start();
                bool isCheckMate = false;
                if (CheckMate.checkIfMate_init(enemyKing, selfKing))
                {
                    isCheckMate = true;
                    output(enemyKing.Color, false, false, false);
                }
                writedownMove(sb1Data.figure, sb2Data, isCheckMate, enemyKing.isCheck);
            }
            BoardPositions.safePosition();
            bool isRemisPositions = Remis.checkPositions();
            bool isRemisMoves = Remis.checkMoves(figure_name);
            if(stalemate || isRemisPositions || isRemisMoves) output("empty", stalemate, isRemisPositions, isRemisMoves);
            positionsCount++;
            positionNumber = positionsCount;
        }

        private bool checkMove(string figure_name, King selfking)
        {
            Chessfigure figure;
            figure = (Chessfigure)(figure_name[0] == 'w' ? whiteFigures : blackFigures).FirstOrDefault(item => item.Item2 == figure_name).Item1;
            if (figure != default) return figure.move(selfking, false);
            return false;
        }

        private bool checkIfCheck(string color)
        {
            King king = color == "white" ? wking1 : bking1;
            return Check.checkIfCheck_init(king.PositionX, king.PositionY, color);
        }

        private void changeColor(King selfKing, King enemyKing, int posx, int posy)
        {
            if (Buttons[posy, posx + (posx > 0 ? -1 : 1)].Background is SolidColorBrush solidColorBrush)
            {
                if(solidColorBrush.Color == Colors.White) Buttons[posy, posx].Background = new SolidColorBrush(Colors.Black);
                else if(solidColorBrush.Color == Colors.Black) Buttons[posy, posx].Background = new SolidColorBrush(Colors.White);
            }

            if (enemyKing.isCheck)
            {
                SolidColorBrush backcol = new SolidColorBrush(Color.FromArgb(135, 255, 0, 0));
                if (enemyKing.isCheck) Buttons[enemyKing.PositionY, enemyKing.PositionX].Background = backcol;
            }
        }

        public void changePawn(CustomData senderData, char col)
        {
            if (Regex.Replace(senderData.figure, @"[\d]", string.Empty) == $"{col}pawn")
            {
                for (int r = 0; r < 8; r++) for (int c = 0; c < 8; c++) Buttons[r, c].IsHitTestVisible = false;
                if(col == 'w')
                {
                    newWFigures.Visibility = Visibility.Visible;
                    TimerMenus.Margin = new Thickness(0, 0, 0, 0);
                }
                else if(col == 'b')
                {
                    newBFigures.Visibility = Visibility.Visible;
                }
            }
            changePawn_Data = senderData;
        }
        public void newFigure_init(object sender, RoutedEventArgs e)
        {
            for (int r = 0; r < 8; r++) for (int c = 0; c < 8; c++) Buttons[r, c].IsHitTestVisible = true;
            if ((sender as Button).Name[0] == 'w') newFigure_white((Button)sender);
            else if ((sender as Button).Name[0] == 'b') newFigure_black((Button)sender);
            TimerMenus.Margin = new Thickness(0, 234, 0, 0);
        }

        private void newFigure_white(Button sender)
        {
            w_timer.Stop();
            b_timer.Start();
            string fig = sender.Name;
            Chessfigure new_fig = null;
            int count = 1;
            foreach ((object, string) entry in whiteFigures)
            {
                string current_fig = Regex.Replace(entry.Item2, @"[\d]", string.Empty);
                if (current_fig == fig) if (int.Parse(Regex.Replace(entry.Item2, @"[a-z]", string.Empty).ToString()) == count) count++;
            }
            string fig_name = sender.Name + count;
            int fig_posx = changePawn_Data.X;
            int fig_posy = changePawn_Data.Y;
            switch (fig)
            {
                case "wqueen":
                    new_fig = new Queen(fig_name, "white", fig_posx, fig_posy, wqueenIMG.Source);
                    break;
                case "wrock":
                    new_fig = new Rock(fig_name, "white", fig_posx, fig_posy, wrockIMG.Source);
                    break;
                case "wbishop":
                    new_fig = new Bishop(fig_name, "white", fig_posx, fig_posy, wbishopIMG.Source);
                    break;
                case "wknight":
                    new_fig = new Knight(fig_name, "white", fig_posx, fig_posy, wknightIMG.Source);
                    break;
            }
            whiteFigures.RemoveAll(entry => entry.Item2 == changePawn_Data.figure);
            Buttons[changePawn_Data.Y, changePawn_Data.X].Content = new Image { Source = new_fig.Image };
            ((CustomData)Buttons[changePawn_Data.Y, changePawn_Data.X].Tag).figure = fig_name;
            whiteFigures.Add((new_fig, fig_name));
            newWFigures.Visibility = Visibility.Collapsed;
            bool isCheckMate = false;
            if (CheckMate.checkIfMate_init(bking1, wking1))
            {
                isCheckMate = true;
                output("black", false, false, false);
            }
            writedownMove(null, changePawn_Data, isCheckMate, bking1.isCheck);
            bking1.isCheck = checkIfCheck(bking1.Color);
            changeColor(wking1, bking1, wking1.PositionX, wking1.PositionY);
        }

        private void newFigure_black(Button sender)
        {
            w_timer.Start();
            b_timer.Stop();
            string fig = sender.Name;
            Chessfigure new_fig = null;
            int count = 1;
            foreach ((object, string) entry in blackFigures)
            {
                string current_fig = Regex.Replace(entry.Item2, @"[\d]", string.Empty);
                if (current_fig == fig) if (int.Parse(Regex.Replace(entry.Item2, @"[a-z]", string.Empty).ToString()) == count) count++;
            }
            string fig_name = sender.Name + count;
            int fig_posx = changePawn_Data.X;
            int fig_posy = changePawn_Data.Y;
            switch (fig)
            {
                case "bqueen":
                    new_fig = new Queen(fig_name, "black", fig_posx, fig_posy, bqueenIMG.Source);
                    break;
                case "brock":
                    new_fig = new Rock(fig_name, "black", fig_posx, fig_posy, brockIMG.Source);
                    break;
                case "bbishop":
                    new_fig = new Bishop(fig_name, "black", fig_posx, fig_posy, bbishopIMG.Source);
                    break;
                case "bknight":
                    new_fig = new Knight(fig_name, "black", fig_posx, fig_posy, bknightIMG.Source);
                    break;
            }
            blackFigures.RemoveAll(entry => entry.Item2 == changePawn_Data.figure);
            Buttons[changePawn_Data.Y, changePawn_Data.X].Content = new Image { Source = new_fig.Image };
            ((CustomData)Buttons[changePawn_Data.Y, changePawn_Data.X].Tag).figure = fig_name;
            blackFigures.Add((new_fig, fig_name));
            newBFigures.Visibility = Visibility.Collapsed;
            bool isCheckMate = false;
            if (CheckMate.checkIfMate_init(wking1, bking1))
            {
                isCheckMate = true;
                output("white", false, false, false);
            }
            writedownMove(null, changePawn_Data, isCheckMate, wking1.isCheck);
            wking1.isCheck = checkIfCheck(wking1.Color);
            changeColor(bking1, wking1, bking1.PositionX, bking1.PositionY);
        }

        private void output(string checkMateColor, bool isStalemate, bool isRemis_RepPos, bool isRemis_Moves)
        {
            resetGame();
            startInteractButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Collapsed;
            restartButton.Width = 200;
            whiteText.FontSize = 20;
            blackText.FontSize = 20;
            if (checkMateColor != "empty")
            {
                if (checkMateColor == "white")
                {
                    whiteText.Text = "#";
                    blackText.Text = "Schwarz gewinnt:\nWeiß ist Schachmatt.";
                }
                else if (checkMateColor == "black")
                {
                    whiteText.Text = "Weiß gewinnt:\nSchwarz ist Schachmatt.";
                    blackText.Text = "#";
                }
            }
            else if (isStalemate)
            {
                whiteText.Text = "Patt";
                blackText.Text = "Patt";
            }
            else if (isRemis_RepPos)
            {
                whiteText.Text = "Remis:\nWiederholungen der Stellung";
                blackText.Text = "Remis:\nWiederholungen der Stellung";
            }
            else if (isRemis_Moves)
            {
                whiteText.Text = "Remis:\n50-Züge Regel";
                blackText.Text = "Remis:\n50-Züge Regel";
            }
        }

        private void resetGame()
        {
            w_timer.Elapsed -= letWhiteTick;
            b_timer.Elapsed -= letBlackTick;
            w_timer.Stop();
            b_timer.Stop();
            startInteractButton.Click -= startTimer_Click;
            startInteractButton.Click -= stopTimer_Click;
        }

        public void restart_Click(object sender, RoutedEventArgs e)
        {
            resetGame();
            countdownWhite = 0;
            countdownBlack = 0;
            for(int i = 0; i < whiteFigures_new.Count; i++) whiteFigures_new[i] = null;
            for(int i = 0; i < blackFigures_new.Count; i++) blackFigures_new[i] = null;
            whiteFigures_new = new List<Object>();
            blackFigures_new = new List<Object>();

            whiteFigures = new List<(Object, string)>();
            blackFigures = new List<(Object, string)>();

            selectedButton1 = null;
            selectedButton2 = null;
            last_move = 'n';
            w_timer = new Timer(1000);
            b_timer = new Timer(1000);
            pause = true;
            beaten_figure = null;
            beaten_figure_test = null;
            stalemate = false;
            changePawn_Data = null;
            initFigures();
            Remis.old_Wfigures = new List<(object, string)>(whiteFigures);
            Remis.old_Bfigures = new List<(object, string)>(blackFigures);
            Remis.counter_Moves = 0;
            BoardPositions.positions = new List<List<(SolidColorBrush, ImageSource)>>();
            BoardPositions.positionCodes = new List<string>();
            positionNumber = 1;
            positionsCount = 1;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Buttons[r, c].Tag = new CustomData { figure = "empty", X = c, Y = r };
                    Buttons[r, c].Content = null;
                    Buttons[r, c].IsHitTestVisible = false;
                }
            }
            Buttons[7, 4].Background = new SolidColorBrush(Colors.Black);
            Buttons[0, 4].Background = new SolidColorBrush(Colors.White);
            startInteractButton.Click += startTimer_Click;
            TimerMenu.Visibility = Visibility.Collapsed;
            TimerStartMenu.Visibility = Visibility.Visible;
            whiteMoveEntries.Clear();
            blackMoveEntries.Clear();
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (positionNumber == positionsCount) return;
            positionNumber++;
            loadPosition(BoardPositions.positions[positionNumber - 1]);
            startInteractButton.IsHitTestVisible = positionNumber == positionsCount ? true : false;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            backButton_Load();
        }

        private void backButton_Load()
        {
            selectedButton1 = null;
            selectedButton2 = null;
            if (positionNumber == 1) return;
            positionNumber--;
            loadPosition(BoardPositions.positions[positionNumber - 1]);
            PauseButton.Visibility = Visibility.Collapsed;
            StartButton.Visibility = Visibility.Visible;
            startInteractButton.IsHitTestVisible = false;
            w_timer.Stop();
            b_timer.Stop();
            pause = true;
            startInteractButton.Click += startTimer_Click;
            startInteractButton.Click -= stopTimer_Click;
        }

        private void loadPosition(List<(SolidColorBrush, ImageSource)> position)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button btn = Buttons[r, c];
                    btn.IsHitTestVisible = false;
                    ((CustomData)btn.Tag).figure = ((CustomData)btn.Tag).figure;
                    btn.Background = position[r * 8 + c].Item1;
                    btn.Content = position[r * 8 + c].Item2 != null ? new Image { Source = position[r * 8 + c].Item2 } : null;
                }
            }
        }

        private void writedownMove(string sb1Fig, CustomData sb2Data, bool isCheckMate, bool isCheck)
        {
            string move = "";
            move += $"#{positionsCount} ";
            if(sb1Fig == null)
            {
                if (sb2Data.figure != "empty") move += 'x';
                move += (char)('a' + sb2Data.X);
                move += 8 - sb2Data.Y;
                move += '=';
                move += char.ToUpper(Regex.Replace(sb2Data.figure, @"[\d]", string.Empty).Substring(1)[0]);
            }
            else
            {
                if (!sb1Fig.Contains("pawn"))
                {
                    move += char.ToUpper(Regex.Replace(sb1Fig, @"[\d]", string.Empty).Substring(1)[0]);
                    move += Regex.Replace(sb1Fig, @"[a-z]", string.Empty);
                }
                if (sb2Data.figure != "empty") move += 'x';
                move += (char)('a' + sb2Data.X);
                move += 8 - sb2Data.Y;
            }
            if (isCheck && !isCheckMate) move += "+";
            else if (isCheckMate) move += "#";
            if (b_timer.Enabled || (isCheckMate && bking1.isCheck)) AddEntryWhite(move);
            else if (w_timer.Enabled || (isCheckMate && wking1.isCheck)) AddEntryBlack(move);
        }

        private void AddEntryWhite(string move)
        {
            currentEntry = new TableEntry(move);
            whiteMoveEntries.Add(currentEntry);
        }

        private void AddEntryBlack(string move)
        {
            currentEntry = new TableEntry(move);
            blackMoveEntries.Add(currentEntry);
        }

        private void WhiteMove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                loadPositionByClick(sender);
            }
        }

        private void BlackMove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                loadPositionByClick(sender);
            }
        }

        private void loadPositionByClick(object sender)
        {
            TextBlock textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                string whiteMove = textBlock.Text;
                int hashIndex = whiteMove.IndexOf('#');
                int posNum = positionNumber;

                if (hashIndex != -1)
                {
                    int spaceIndex = whiteMove.IndexOf(' ', hashIndex);

                    string numberPart = (spaceIndex != -1)
                        ? whiteMove.Substring(hashIndex + 1, spaceIndex - hashIndex - 1)
                        : whiteMove.Substring(hashIndex + 1);

                    if (int.TryParse(numberPart, out int extractedNumber))
                    {
                        posNum = extractedNumber;
                    }
                    if (posNum + 1 == positionsCount && positionNumber == positionsCount) return;
                    posNum += 2;
                    positionNumber = posNum;
                    backButton_Load();
                    if(positionNumber == positionsCount) startInteractButton.IsHitTestVisible = true;
                }
            }
        }
    }
}
