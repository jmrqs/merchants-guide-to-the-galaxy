using System.Text.RegularExpressions;

namespace MerchantsGuideToTheGalaxy.Service.Helpers
{
    public static partial class ConversorNumeroRomanos
    {
        [GeneratedRegex(@"^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$", 
            RegexOptions.IgnoreCase)]
        private static partial Regex ValidacaoNumeroRomanoRegex();

        public static bool TransformarEmNumeroArabico(ReadOnlySpan<char> roman, out int resultado)
        {
            resultado = 0;

            if (roman.IsWhiteSpace() || !ValidacaoNumeroRomanoRegex().IsMatch(roman.ToString()))
                return false;

            int total = 0;
            for (int i = 0; i < roman.Length; i++)
            {
                int atual = GetValue(roman[i]);
                int proximo = (i + 1 < roman.Length) ? GetValue(roman[i + 1]) : 0;

                if (atual < proximo)
                    total -= atual;
                else
                    total += atual;
            }

            resultado = total;
            return true;
        }

        private static int GetValue(char c) => c switch
        {
            'I' => 1,
            'V' => 5,
            'X' => 10,
            'L' => 50,
            'C' => 100,
            'D' => 500,
            'M' => 1000,
            _ => 0
        };
    }
}