using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static NaughtsAndCrossesGenerator.Bot.Round;

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

        /// <summary>
        /// Trains the bot based on the last round and then saves the file
        /// </summary>
        /// <param name="round">The last round</param>
        public void TrainOnRound(Round round)
        {
            Debug.WriteLine("Train called");
            rounds.Add(round);

            Debug.WriteLine("Round won: " + round.DetermineIfWon().ToString());

            if (round.DetermineIfWon() == true)
            {
                for (int i = 0; i < round.moves.Count; i++)
                {
                    if (round.moves[i].wasPlayer == false)
                    {
                        if(i == 0)
                        {
                            Round.Move move = round.moves[i];
                            for (int x = 0; x < 3; x++)
                            {
                                for (int y = 0; y < 3; y++)
                                {
                                    // Since 2 is crosses (The bot in this case)
                                    if (move.map[x, y] == 2 && picks[x, y] != 100.0 && picks[x, y] + Global.BotAdd <= 100)
                                    {
                                        picks[x, y] += Global.BotAdd;
                                    }
                                }
                            }
                        } else
                        {
                            Round.Move move = RemoveExisting(round.moves[i - 1], round.moves[i]);
                            for (int x = 0; x < 3; x++)
                            {
                                for (int y = 0; y < 3; y++)
                                {
                                    // Since 2 is crosses (The bot in this case)
                                    if(move.map[x, y] == 2 && picks[x, y] != 100.0 && picks[x, y] + Global.BotAdd <= 100)
                                    {
                                        picks[x, y] += Global.BotAdd;
                                    }
                                }
                            }
                        }
                    }
                }
            } else if (round.DetermineIfWon() == false)
            {
                for (int i = 0; i < round.moves.Count; i++)
                {
                    if (round.moves[i].wasPlayer == false)
                    {
                        if(i == 0)
                        {
                            Round.Move move = round.moves[i];
                            for (int x = 0; x < 3; x++)
                            {
                                for (int y = 0; y < 3; y++)
                                {
                                    // Since 2 is crosses (The bot in this case)
                                    if (move.map[x, y] == 2 && picks[x, y] != 100.0 && picks[x, y] + Global.BotTakeaway <= 100)
                                    {
                                        picks[x, y] += Global.BotTakeaway;
                                    }
                                }
                            }
                        } else
                        {
                            Round.Move move = RemoveExisting(round.moves[i - 1], round.moves[i]);
                            for (int x = 0; x < 3; x++)
                            {
                                for (int y = 0; y < 3; y++)
                                {
                                    // Since 2 is crosses (The bot in this case)
                                    if (move.map[x, y] == 2 && picks[x, y] != 0.0 && picks[x, y] - Global.BotTakeaway >= 0)
                                    {
                                        picks[x, y] += Global.BotTakeaway;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if(DetermineIfAllAreZero(picks))
            {
                MessageBox.Show("Something went wrong while training the bot: All of the values are set to 0.0");
            }

            Global.mainWindow.botInfo.Save();
        }

        /// <summary>
        /// Determines if everything in the grid is set to 0.
        /// </summary>
        /// <param name="picks">The grid</param>
        /// <returns>`true` if They all are zero, 'false' if not.</returns>
        public bool DetermineIfAllAreZero(double[,] picks)
        {
            bool areZero = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (picks[i, j] != 0.0) areZero = false;
                }
            }
            return areZero;
        }

        /// <summary>
        /// Removes any existing points on the map, leaving behind only the latest one.
        /// </summary>
        /// <param name="last">The move before the last</param>
        /// <param name="current">The last move</param>
        /// <returns>A move with only the latest point.</returns>
        public Round.Move RemoveExisting(Round.Move last, Round.Move current)
        {
            Round.Move filtered = new Round.Move()
            {
                wasPlayer = current.wasPlayer
            };

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (last.map[x, y] == 0 && current.map[x, y] != 0)
                    {
                        filtered.map[x, y] = current.map[x, y];
                    }
                }
            }

            return current;
        }

        /// <summary>
        /// Work out the next move by the bot.
        /// </summary>
        /// <param name="round">the current round</param>
        /// <returns>The x and y coordinates of the position</returns>
        public int[] MakeMove(Round round)
        {
            // Work out based on the values in the picks 3D-Array which ones should be picked,
            // then generate the random numbers. Work out which one is within the threshold,
            // if none, then redo it (But with a lower threshold).
            // Repeat this until it has picked a value, then match it against the latest move
            // and make sure that it is not overlapping any other moves, if not, return the
            // value, if it is, redo.

            int threshold = 0; // The minimum, across all of the selections
            bool moveSelected = false;
            int[] move = new int[2];

            while(!moveSelected)
            {
                Random r = new Random();

                bool[,] bools = new bool[3, 3];

                /*bool A1 = r.Next(threshold, (int)(100.00 - picks[0, 0])) > (threshold * 1.5);
                bool A2 = r.Next(threshold, (int)(100.00 - picks[0, 1])) > (threshold * 1.5);
                bool A3 = r.Next(threshold, (int)(100.00 - picks[0, 2])) > (threshold * 1.5);

                bool B1 = r.Next(threshold, (int)(100.00 - picks[1, 0])) > (threshold * 1.5);
                bool B2 = r.Next(threshold, (int)(100.00 - picks[1, 1])) > (threshold * 1.5);
                bool B3 = r.Next(threshold, (int)(100.00 - picks[1, 2])) > (threshold * 1.5);

                bool C1 = r.Next(threshold, (int)(100.00 - picks[2, 0])) > (threshold * 1.5);
                bool C2 = r.Next(threshold, (int)(100.00 - picks[2, 1])) > (threshold * 1.5);
                bool C3 = r.Next(threshold, (int)(100.00 - picks[2, 2])) > (threshold * 1.5);*/

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        //bools[x, y] = r.Next(threshold, (int)(100.00 - picks[x, y])) > (threshold * 1.5);
                        bools[x, y] = r.Next(threshold, (int)picks[x, y]) > (threshold * 1.5);

                        if (round.moves[round.moves.Count - 1].map[x, y] == 0 && bools[x, y])
                        {
                            return [x, y];
                        }
                    }
                }

                threshold += 1;
            }

            return [-1, -1]; // (-1, -1) = No move, failed basically
        }
    }
}
