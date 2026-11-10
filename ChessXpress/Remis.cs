using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessXpress
{
    internal class Remis
    {
        public static List<(object, string)> old_Wfigures = new List<(object, string)>(MainWindow.whiteFigures);
        public static List<(object, string)> old_Bfigures = new List<(object, string)>(MainWindow.blackFigures);
        public static int counter_Moves = 0;

        public static bool checkPositions()
        {
            int counter = 0;
            List<string> positions = BoardPositions.positionCodes;
            for(int i = 1; i < positions.Count; i++)
            {
                if (positions[i] == positions[positions.Count - 1]) counter++;
                if (counter == 3) return true;
            }
            return false;
        }
        public static bool checkMoves(string figure)
        {
            if (figure.Substring(1, figure.Length - 2) == "pawn")
            {
                resetMoves();
                return false;
            }

            if ((figure[0] == 'w' && !MainWindow.blackFigures.SequenceEqual(old_Bfigures))
                || (figure[0] == 'b' && !MainWindow.whiteFigures.SequenceEqual(old_Wfigures)))
            {
                resetMoves();
                return false;
            }

            counter_Moves++;
            if (counter_Moves == 100) return true;
            return false;
        }
        private static void resetMoves()
        {
            old_Wfigures = new List<(object, string)>(MainWindow.whiteFigures);
            old_Bfigures = new List<(object, string)>(MainWindow.blackFigures);
            counter_Moves = 0;
        }
    }
}
