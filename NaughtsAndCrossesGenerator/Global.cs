using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaughtsAndCrossesGenerator
{
    public static class Global
    {
        public static MainWindow mainWindow;
        public static bool IndentJSONFiles = true;

        // For the bot training (Adjust values from winning or losing, but not if a tie)
        public static double BotAdd      = 0.1;
        public static double BotTakeaway = 0.1;
    }
}
