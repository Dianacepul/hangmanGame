using System.Collections.Generic;
using System.Linq;

namespace HangmanMyra
{
    internal class Level
    {
        static private List<char> letters = new List<char> { 'E', 'A', 'R', 'I', 'O', 'T', 'N', 'S', 'L', 'C', 'U', 'D', 'P', 'M', 'H', 'G', 'B', 'F', 'Y', 'W', 'K', 'V', 'X', 'Z', 'J', 'Q' };

        public static string LevelDefinition(string wordToSpell)
        {
            int lettersIndexSum = wordToSpell.Sum(c => letters.IndexOf(char.ToUpper(c)));

            if (lettersIndexSum <= 36)
            {
                return "Easy";
            }
            else if (lettersIndexSum >= 37 && lettersIndexSum <= 117)
            {
                return "Medium";
            }
            else
            {
                return "Hard";
            }
        }
    }
}