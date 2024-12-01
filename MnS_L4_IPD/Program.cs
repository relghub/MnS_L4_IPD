namespace MnS_L4_IPD
{
    internal partial class Program
    {
        public static readonly Random rnd = new();
        public static byte typeOne = 0, typeTwo = 0, typeThree = 0, typeFour = 0;
        public static string[] playerTypes = ["Cooperator", "Defector", "Randomizer", "Tit-for-tater"];
        public static byte[] playerCount = [typeOne, typeTwo, typeThree, typeFour];

        public static List<DilemmaPlayer> players = [];

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
            Play(mode);
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
        static void Play(bool mode)
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
                    playerValues.Clear();
                    enemyValues.Clear();
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
                        enemyValues);
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
