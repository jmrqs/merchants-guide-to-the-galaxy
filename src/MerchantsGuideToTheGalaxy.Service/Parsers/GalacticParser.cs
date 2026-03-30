using System.Text.RegularExpressions;

namespace MerchantsGuideToTheGalaxy.Service.Parsers
{
    public static partial class GalacticParser
    {
        [GeneratedRegex(@"^([a-z]+)\s+é\s+([IVXLCDM])$", RegexOptions.IgnoreCase)]
        public static partial Regex MapeamentoNumeroRomano();

        [GeneratedRegex(@"^(.+) ([A-Z][a-z]+) é (\d+) Créditos$", RegexOptions.IgnoreCase)]
        public static partial Regex MapeamentoMetal();

        [GeneratedRegex(@"^quanto\s+é\s+(.+)\?$", RegexOptions.IgnoreCase)]
        public static partial Regex QuantoE();

        [GeneratedRegex(@"^quantos Créditos é (.+)\?$", RegexOptions.IgnoreCase)]
        public static partial Regex QuantosCreditosE();
    }
}
