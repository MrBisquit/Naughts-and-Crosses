﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaughtsAndCrossesGenerator.Bot
{
    public class Round
    {
        bool? won = false; // Null = Tie
        public List<Move> moves = new List<Move>();
        public int winner = 0; // For other purposes
        public class Move
        {
            public bool wasPlayer;
            public int[,] map = new int[3, 3];
        }

        /// <summary>
        /// Determines if the round was won by the bot or not
        /// </summary>
        /// <returns>'true' if the bot won, 'false' if it did not and 'null' if it was a tie.</returns>
        public bool? DetermineIfWon()
        {
            // Naughts (Noughts) - 'o' = 1
            // Crosses           - 'x' = 2 | Bot (If playing against the bot)
            if (winner == 2) won = true;
            if (winner == 1) won = false;
            else won = null;

            return won;
        }
    }
}
