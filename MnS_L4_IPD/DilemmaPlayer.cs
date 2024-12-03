namespace MnS_L4_IPD
{
    internal partial class Program
    {
        public class DilemmaPlayer(byte pType)
        {

            private bool playerValue = true;
            private byte playerType = pType;
            public bool isFirst = true;
            public int playerScore;

            private readonly Random rnd = new();

            public byte Type()
            {
                return playerType;
            }
            public void Type(byte? type)
            {
                if (type is byte newType)
                {
                    playerType = newType;
                }
            }
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
                    playerValue = true;
                    isFirst = false;
                }
                else if (enemyValue is bool eValue)
                {
                    playerValue = !eValue ? eValue : eValue;
                }
                return playerValue;
            }

            public static (int, int) Score(ref int playerScore, ref int enemyScore, int iterations, List<bool> playerValues, List<bool> enemyValues, bool mode)
            {
                if (mode)
                {
                    playerScore = 0;
                    enemyScore = 0;
                }
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
