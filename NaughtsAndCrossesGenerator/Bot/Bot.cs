using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaughtsAndCrossesGenerator.Bot
{
    public class Bot
    {
        // It's basically random numbers but it is less likely to choose the ones that lead it to lose
        // previous rounds.
        public double[,] picks = new double[3, 3]
        {
            {
                100.0,
                100.0,
                100.0
            },
            {
                100.0,
                100.0,
                100.0,
            },
            {
                100.0,
                100.0,
                100.0
            }
        };

        public int wins;
        public int losses;
        public List<Round> rounds = new List<Round>();
        // The user selects the file location and this will save to it every time there is an update
        public void TrainOnRound(Round round)
        {
            rounds.Add(round);
        }
    }
}
