using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day2
    {
        public static string A(string input)
        {
            List<Tuple<RPS, RPS>> games = new List<Tuple<RPS, RPS>>();
            foreach (string line in input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                //C Z
                string[] parts = line.Split(new char[] { ' ' });
                RPS first;
                RPS second;
                switch (parts[0])
                {
                    case "A":
                        first = RPS.Rock;
                        break;
                    case "B":
                        first = RPS.Paper;
                        break;
                    case "C":
                        first = RPS.Scissors;
                        break;
                    default: throw new Exception("Invalid Play");
                }
                switch (parts[1])
                {
                    case "X":
                        second = RPS.Rock;
                        break;
                    case "Y":
                        second = RPS.Paper;
                        break;
                    case "Z":
                        second = RPS.Scissors;
                        break;
                    default: throw new Exception("Invalid Play");
                }

                games.Add(new Tuple<RPS, RPS>(second, first));

            }

            int score = 0;
            foreach (Tuple<RPS, RPS> game in games)
            {
                Result result = RPSCalcResult(game.Item1, game.Item2);
                int moveScore = 0;
                //1 for Rock, 2 for Paper, and 3 for Scissors
                switch (game.Item1)
                {
                    case RPS.Rock:
                        moveScore  =1;
                        break;
                    case RPS.Paper:
                        moveScore = 2;
                        break;
                    case RPS.Scissors:
                        moveScore = 3;
                        break;
                    default:
                        throw new Exception("Invalid play");
                }
                int resultScore = 0;
                switch (result)
                {
                    case Result.Win:
                        resultScore= 6;
                        break;
                    case Result.Draw:
                        resultScore= 3;
                        break;
                    case Result.Lose:
                        break;

                    default:
                        throw new Exception("Invalid play");
                }
                score = score + moveScore + resultScore;
            }
            return score.ToString();
        }

        public static string B(string input)
        {
            List<Tuple<RPS, Result>> games = new List<Tuple<RPS, Result>>();
            foreach (string line in input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                //C Z
                string[] parts = line.Split(new char[] { ' ' });
                RPS first;
                Result second;
                switch (parts[0])
                {
                    case "A":
                        first = RPS.Rock;
                        break;
                    case "B":
                        first = RPS.Paper;
                        break;
                    case "C":
                        first = RPS.Scissors;
                        break;
                    default: throw new Exception("Invalid Play");
                }
                switch (parts[1])
                {
                    case "X":
                        second = Result.Lose;
                        break;
                    case "Y":
                        second = Result.Draw;
                        break;
                    case "Z":
                        second = Result.Win;
                        break;
                    default: throw new Exception("Invalid Play");
                }

                games.Add(new Tuple<RPS, Result>(first, second));
            }
            int score = 0;
            foreach (Tuple<RPS,Result> game in games)
            {

                RPS me = RPSCalcPlay (game.Item1, game.Item2);
                int moveScore = 0;
                //1 for Rock, 2 for Paper, and 3 for Scissors
                switch (me)
                {
                    case RPS.Rock:
                        moveScore = 1;
                        break;
                    case RPS.Paper:
                        moveScore = 2;
                        break;
                    case RPS.Scissors:
                        moveScore = 3;
                        break;
                    default:
                        throw new Exception("Invalid play");
                }
                int resultScore = 0;
                switch (game.Item2)
                {
                    case Result.Win:
                        resultScore = 6;
                        break;
                    case Result.Draw:
                        resultScore = 3;
                        break;
                    case Result.Lose:
                        break;

                    default:
                        throw new Exception("Invalid play");
                }
                score = score + moveScore + resultScore;
            }
            return score.ToString();
        }


        private static Result RPSCalcResult(RPS me, RPS them)
        {
            switch (me)
            {
                case RPS.Rock:
                    switch (them)
                    {
                        case RPS.Rock:
                            return Result.Draw;
                        case RPS.Paper:
                            return Result.Lose;
                        case RPS.Scissors:
                            return Result.Win;
                        default: throw new Exception("Invalid Play");
                    }
                case RPS.Paper:
                    switch (them)
                    {
                        case RPS.Rock:
                            return Result.Win;
                        case RPS.Paper:
                            return Result.Draw;
                        case RPS.Scissors:
                            return Result.Lose;
                        default: throw new Exception("Invalid Play");
                    }
                case RPS.Scissors:
                    switch (them)
                    {
                        case RPS.Rock:
                            return Result.Lose;
                        case RPS.Paper:
                            return Result.Win;
                        case RPS.Scissors:
                            return Result.Draw;
                        default: throw new Exception("Invalid Play");
                    }
                default: throw new Exception("Invalid Play");

            }
        }

        private static RPS RPSCalcPlay(RPS them, Result result)
        {
            switch (them)
            { 
                case RPS.Rock:
                switch (result)
                    {
                        case Result.Lose:
                            return RPS.Scissors;
                        case Result.Draw:
                            return them;
                        case Result.Win:
                            return RPS.Paper;
                        default: throw new Exception("Invalid play");
                    }
                case RPS.Paper:
                    switch(result)
                    {
                        case Result.Lose:
                            return RPS.Rock;
                        case Result.Draw:
                            return them;
                        case Result.Win:
                            return RPS.Scissors;
                        default: throw new Exception("Invalid play");
                    }
                case RPS.Scissors:
                    switch (result)
                    {
                        case Result.Lose:
                            return RPS.Paper;
                        case Result.Draw:
                            return them;
                        case Result.Win:
                            return RPS.Rock;
                        default: throw new Exception("Invalid play");
                    }
                default: throw new Exception("Invalid play");
            }
        }

        private enum RPS
        {
            Rock,
            Paper,
            Scissors
        }

        private enum Result
        {
            Win,
            Draw,
            Lose
        }
    }
}
