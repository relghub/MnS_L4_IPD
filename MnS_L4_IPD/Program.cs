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
			SelectPlayer();
		}

		static void Count(byte[] playerCount, List<DilemmaPlayer> players)
		{
			for (int i = 0; i < playerCount.Length; i++)
			{
				int? first = null;
				int? last = null;
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                first = players.FindIndex(x => x.ShowType() == i);
                last = players.FindLastIndex(x => x.ShowType() == i);
                if (first == -1 || last == -1) {}
				else if (first == last)
				{
                    Console.Write($"{playerTypes[i]} players are at number {++first}");
                }
				else if (first != last)
                {
                    Console.Write($"{playerTypes[i]} players are at numbers {++first} - {++last}");
                }
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top + 1);
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

		static void SelectPlayer()
		{
			Console.Write("Enter the number of player: ");
			byte player = (byte)(byte.Parse(Console.ReadLine()) - 1);
			ShowPlayerType(player);
			Play(player);
		}

		static void ShowPlayerType(byte number)
		{
			Console.Write($"Player type: {playerTypes[players[number].ShowType()]}");
		}

		static void Play(byte playerNumber)
		{
			bool playerValue = true, enemyValue = true;
			bool? prevPValue = null, prevEValue = null;
			byte enemyNumber;
			(int, int) scores;
			while (true)
			{
				enemyNumber = (byte)rnd.Next(players.Count);
				if (enemyNumber != playerNumber)
				{
					Console.Write(Environment.NewLine
						+ $"Enemy number: {enemyNumber + 1}"
						+ Environment.NewLine);
					ShowPlayerType(enemyNumber);
					break;
				}
			}
			int iterationCount = 10;
			List<bool> playerValues = [], enemyValues = [];
			Console.Write(Environment.NewLine);
			for (int i = 0; i < iterationCount; i++)
			{
				Display(ref playerValue, playerNumber, ref playerValues, prevEValue);
				if (i == 0)
				{
					Console.Write(Environment.NewLine);
				}
				else
				{
					Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top + 1);
				}
				Display(ref enemyValue, enemyNumber, ref enemyValues, prevPValue);
				prevPValue = playerValue;
				prevEValue = enemyValue;
				Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top - 1);
			}
			Console.SetCursorPosition(Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top + 1);
			Console.Write(Environment.NewLine);
			scores = DilemmaPlayer.Score(
				ref players[playerNumber].playerScore,
				ref players[enemyNumber].playerScore,
				iterationCount,
				playerValues,
				enemyValues);

			Console.WriteLine($"Player score: {scores.Item1}");
			Console.WriteLine($"Enemy score: {scores.Item2}");
		}

		static void Display(
			ref bool playerValue,
			byte playerNumber,
			ref List<bool> playerValues,
			bool? prevPlayerValue)
		{
			switch (playerValue = players[playerNumber].PlayerFunction(players[playerNumber].ShowType(),
																	   ref prevPlayerValue))
			{
				case true: Console.Write('C'); playerValues.Add(playerValue); break;
				case false: Console.Write('D'); playerValues.Add(playerValue); break;
				default: throw new ArgumentOutOfRangeException();
			}
		}
	}
}
