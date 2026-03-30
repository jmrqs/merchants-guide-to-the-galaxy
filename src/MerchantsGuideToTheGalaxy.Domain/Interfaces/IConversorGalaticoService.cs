namespace MerchantsGuideToTheGalaxy.Domain.Interfaces
{
    public interface IConversorGalaticoService
    {
        Task<string> ProcessarConversaoAsync(string entrada);
    }
}
