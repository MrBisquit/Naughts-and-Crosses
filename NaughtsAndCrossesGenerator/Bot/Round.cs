using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaughtsAndCrossesGenerator.Bot
{
    public class Round
    {
        bool won;
        public List<Move> moves = new List<Move>();
        public class Move
        {
            public bool wasPlayer;
            int[,] map = new int[3, 3];
        }
    }
}
