namespace MnS_L4_IPD
{
    internal partial class Program
    {
        public class DilemmaPlayer(byte pType)
        {
            private readonly byte playerType = pType;
            public int playerScore;

            public byte ShowType()
            {
                return playerType;
            }

            private bool playerValue = true;
            private bool isFirst = true;
            private readonly Random rnd = new();

            public bool PlayerFunction(byte pType, ref bool? enemyValue)
            {
                var value = pType switch
                {
                    0 => Cooperate,
                    1 => Defect,
                    2 => Randomize,
                    3 => TitForTat(enemyValue),
                    _ => throw new ArgumentOutOfRangeException(nameof(pType))
                };
                return value;
            }

            public bool Cooperate
            {
                get
                { playerValue = true; return playerValue; }
            }

            public bool Defect
            {
                get
                { playerValue = false; return playerValue; }
            }

            public bool Randomize
            {
                get
                { playerValue = Math.Round(rnd.NextSingle()) == 1; return playerValue; }
            }

            public bool TitForTat(bool? enemyValue)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else if (enemyValue is bool eValue)
                {
                    playerValue = !eValue ? eValue : eValue;
                }
                return playerValue;
            }

            public static (int, int) Score(ref int playerScore, ref int enemyScore, int iterations, List<bool> playerValues, List<bool> enemyValues)
            {
                for (int i = 0; i < iterations; i++)
                {
                    switch ((playerValues[i], enemyValues[i]))
                    {
                        case (true, true):
                            playerScore += 4;
                            enemyScore += 4;
                            break;
                        case (false, false):
                            playerScore += 2;
                            enemyScore += 2;
                            break;
                        case (true, false):
                            playerScore += 0;
                            enemyScore += 5;
                            break;
                        case (false, true):
                            playerScore += 5;
                            enemyScore += 0;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                return (playerScore, enemyScore);
            }
        }
    }
}
