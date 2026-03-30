namespace MerchantsGuideToTheGalaxy.Service.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> QuebrarEmLinhas(this string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada)) return[];

            return entrada.Split(["\r\n", "\r", "\n"], StringSplitOptions.RemoveEmptyEntries);
        }
    }
}