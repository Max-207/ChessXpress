using NSKing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using NSFigure;
namespace ChessXpress
{
    public class BoardPositions
    {
        public static List<List<(SolidColorBrush, ImageSource)>> positions = new List<List<(SolidColorBrush, ImageSource)>>();
        public static List<string> positionCodes = new List<string>();

        public static List<(SolidColorBrush, ImageSource)> position = new List<(SolidColorBrush, ImageSource)>();
        public static string positionCode = "";

        public static void safePosition()
        {
            position = new List<(SolidColorBrush, ImageSource)>();
            positionCode = "";
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    ImageSource imgsrc = (Image)MainWindow.Buttons[r, c].Content == null ? null : ((Image)MainWindow.Buttons[r, c].Content).Source;
                    position.Add(((SolidColorBrush)MainWindow.Buttons[r, c].Background, imgsrc));
                    positionCode += ((CustomData)MainWindow.Buttons[r, c].Tag).figure;
                }
            }
            positions.Add(position);
            positionCodes.Add(positionCode);
        }
    }
}