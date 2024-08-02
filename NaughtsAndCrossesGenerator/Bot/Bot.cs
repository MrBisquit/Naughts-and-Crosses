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
    }
}
