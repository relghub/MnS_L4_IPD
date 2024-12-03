namespace MnS_L4_IPD
{
    internal partial class Program
    {
        private static readonly Random rnd = new();
        private static bool playMode = false;
        private static byte typeOne = 0, typeTwo = 0, typeThree = 0, typeFour = 0;
        private static int scoreT1 = 0, scoreT2 = 0, scoreT3 = 0, scoreT4 = 0;
        private static string[] playerTypes = ["Cooperator", "Defector", "Randomizer", "Tit-for-tater"];
        private static byte[] playerCount = [typeOne, typeTwo, typeThree, typeFour];
        private static int[] playerScoresByType = [scoreT1, scoreT2, scoreT3, scoreT4];

        private static List<DilemmaPlayer> players = [];

        static void Main(string[] args)
        {
            bool mode = false;

            Console.SetCursorPosition((int)double.Truncate((Console.WindowWidth - "Welcome to PDin-PDep!".Length) / 2), 0);
            Console.WriteLine("Welcome to PDin-PDep!");
            Thread.Sleep(1000);
            Console.SetCursorPosition((int)double.Truncate((Console.WindowWidth - "The Prisoner's Dilemma in the Police Department".Length) / 2), 0);
            Console.WriteLine("The Prisoner's Dilemma in the Police Department");
            Console.Write(Environment.NewLine);
            while (true)
            {
                int i = 0;
                try
                {
                    for (i = 0; i < playerCount.Length; i++)
                    {
                        Console.Write(string.Format("Enter the number of Type {0} players: ", i + 1));
                        playerCount[i] = byte.Parse(Console.ReadLine());
                        Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                    Console.Write("Invalid input! Try again!");
                    Thread.Sleep(1000);
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top - (i + 1));
                }
            }
            FillPlayerPool(playerCount, players);
            Count(playerCount, players);

            while (true)
            {
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                Console.Write("Enable evaluation logs? ");
                try
                {
                    switch (Console.ReadLine().ToLower())
                    {
                        case "yes":
                        case "y":
                        case "1":
                            mode = true;
                            break;
                        case "no":
                        case "n":
                        case "0":
                            mode = false;
                            break;
                        default:
                            throw new FormatException();
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                    Console.Write("Invalid input! Try again!");
                    Thread.Sleep(1000);
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                }
            }
            while (true)
            {
                Console.Write("Play between existing players or add a new one? ");
                try
                {
                    switch (Console.ReadLine().ToLower())
                    {
                        case "yes":
                        case "y":
                        case "1":
                            playMode = true;
                            break;
                        case "no":
                        case "n":
                        case "0":
                            playMode = false;
                            break;
                        default:
                            throw new FormatException();
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                    Console.Write("Invalid input! Try again!");
                    Thread.Sleep(1000);
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                }
            }
            switch (playMode)
            {
                case false:
                    Play(mode); break;
                case true:
                    PlayWithANewPlayer(mode); break;
            }
        }
        static void Count(byte[] playerCount, List<DilemmaPlayer> players)
        {
            for (int i = 0; i < playerCount.Length; i++)
            {
                int? first = null;
                int? last = null;
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                first = players.FindIndex(x => x.Type() == i);
                last = players.FindLastIndex(x => x.Type() == i);
                if (first == -1 || last == -1) { }
                else if (first == last)
                {
                    Console.Write($"{playerTypes[i]} player is at position {++first}");
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top + 1);
                }
                else if (first != last)
                {
                    Console.Write($"{playerTypes[i]} players are at positions {++first} - {++last}");
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top + 1);
                }
            }
        }
        static void FillPlayerPool(byte[] playerCount, List<DilemmaPlayer> players)
        {
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top - playerCount.Length);
            byte countIndex;

            for (countIndex = 0; countIndex < playerCount.Length; countIndex++)
            {
                byte countAtIndex = playerCount[countIndex];
                for (byte i = 0; i < countAtIndex; i++)
                {
                    players.Add(new DilemmaPlayer(countIndex));
                }
            }
        }
        static void PlayWithANewPlayer(bool mode)
        {
            bool playerValue = true, enemyValue = true;
            bool? prevPValue = null, prevEValue = null;
            byte playerNumber = (byte)(players.Count);
            int iterationCount = 10;
            int playerScoreSum = 0;

            (int, int) scores;
            List<bool> playerValues = [], enemyValues = [];
            List<int> playerScores = [];

            players.Add(new DilemmaPlayer(0));
            Console.Write(Environment.NewLine);
            for (byte? h = 0; h < playerTypes.Length; h++)
            {
                players[playerNumber].Type(h);
                playerScoreSum = 0;
                for (int i = 0; i < players.Count - 1; i++)
                {
                    if (mode)
                    {
                        Console.WriteLine($"Player {playerNumber + 1} - Player {i + 1}");
                    }
                    playerValues.Clear();
                    enemyValues.Clear();
                    players[playerNumber].isFirst = true;
                    players[i].isFirst = true;
                    for (int j = 0; j < iterationCount; j++)
                    {
                        Display(ref playerValue, playerNumber, ref playerValues, prevEValue);
                        Display(ref enemyValue, (byte)i, ref enemyValues, prevPValue);
                        prevPValue = playerValue;
                        prevEValue = enemyValue;
                    }
                    scores = DilemmaPlayer.Score(
                        ref players[playerNumber].playerScore,
                        ref players[i].playerScore,
                        iterationCount,
                        playerValues,
                        enemyValues,
                        true);
                    if (mode)
                    {
                        foreach (bool p in playerValues)
                        {
                            Console.Write(p ? 'C' : 'D');
                        };
                        Console.Write(Environment.NewLine);
                        foreach (bool e in enemyValues)
                        {
                            Console.Write(e ? 'C' : 'D');
                        }
                        Console.Write(Environment.NewLine);
                        Console.WriteLine($"{scores.Item1}:{scores.Item2}");
                    }
                    playerScoreSum += scores.Item1;
                }
                playerScores.Add(playerScoreSum);
                Console.WriteLine($"Player score as a {playerTypes[(int)h]}: {playerScores[(int)h]}");
                Console.Write(Environment.NewLine);
            }
            Console.WriteLine($"Playing as a {playerTypes[playerScores.IndexOf(playerScores.Max())]} provides the best score in this scenario.");
        }

        static void Play(bool mode)
        {
            bool playerValue = true, enemyValue = true;
            bool? prevPValue = null, prevEValue = null;
            int iterationCount = 10;

            (int, int) scores;
            List<bool> playerValues = [], enemyValues = [];
            List<int> playerScores = [];

            Console.Write(Environment.NewLine);
            for (int h = 0; h < players.Count; h++)
            {
                players[h].isFirst = true;
                for (int i = 0; i < players.Count; i++)
                {
                    if (h < i)
                    {
                        if (mode)
                        {
                            Console.WriteLine($"Player {h + 1} - Player {i + 1}");
                        }
                        playerValues.Clear();
                        enemyValues.Clear();
                        players[i].isFirst = true;
                        for (int j = 0; j < iterationCount; j++)
                        {

                            Display(ref playerValue, (byte)h, ref playerValues, prevEValue);
                            Display(ref enemyValue, (byte)i, ref enemyValues, prevPValue);
                            prevPValue = playerValue;
                            prevEValue = enemyValue;
                        }
                        scores = DilemmaPlayer.Score(
                            ref players[h].playerScore,
                            ref players[i].playerScore,
                            iterationCount,
                            playerValues,
                            enemyValues,
                            false);
                        if (mode)
                        {
                            foreach (bool p in playerValues)
                            {
                                Console.Write(p ? 'C' : 'D');
                            };
                            Console.Write(Environment.NewLine);
                            foreach (bool e in enemyValues)
                            {
                                Console.Write(e ? 'C' : 'D');
                            }
                            Console.Write(Environment.NewLine);
                            Console.WriteLine($"{scores.Item1}:{scores.Item2}");
                        }
                    }
                    else { continue; }
                }
                if (mode)
                {
                    Console.WriteLine($"Player {h + 1} score is {players[h].playerScore}");
                }
            }
            PlayerTraverse();
        }

        static void PlayerTraverse()
        {
            int j = 0;
            for (int i = 0; i < playerScoresByType.Length; i++)
            {
                if (j < players.Count)
                {
                    foreach (DilemmaPlayer player in players[j..])
                    {
                        if (player.Type() != i)
                        {
                            break;
                        }
                        playerScoresByType[i] += player.playerScore;
                        j++;
                    }
                }
                if (playerCount[i] != 0)
                {
                    playerScoresByType[i] /= playerCount[i];
                    Console.WriteLine($"Average score of {playerTypes[i].ToLower()} players is: {playerScoresByType[i]}");
                }
            }
        }
        static void Display(
            ref bool playerValue,
            byte playerNumber,
            ref List<bool> playerValues,
            bool? prevPlayerValue)
        {
            switch (playerValue = players[playerNumber].PlayerFunction(players[playerNumber].Type(),
                                                                       ref prevPlayerValue))
            {
                case true: playerValues.Add(playerValue); break;
                case false: playerValues.Add(playerValue); break;
                default: throw new ArgumentOutOfRangeException();
            }

        }
    }
}
