namespace MerchantsGuideToTheGalaxy.Service
{
    using MerchantsGuideToTheGalaxy.Domain;
    using MerchantsGuideToTheGalaxy.Domain.Interfaces;
    using MerchantsGuideToTheGalaxy.Service.Extensions;
    using MerchantsGuideToTheGalaxy.Service.Helpers;
    using MerchantsGuideToTheGalaxy.Service.Parsers;
    using System.Collections.Concurrent;

    public class ConversorGalaticoService : IConversorGalaticoService
    {
        private readonly ConcurrentDictionary<string, string> _vocabulario = new();
        private readonly ConcurrentDictionary<string, decimal> _precosMetais = new();

        public async Task<string> ProcessarConversaoAsync(string entrada)
        {
            return await Task.Run(() =>
            {
                var linhas = entrada.QuebrarEmLinhas();
                var respostas = new List<string>();

                foreach (var linha in linhas)
                {
                    var trim = linha.Trim();
                    if (string.IsNullOrWhiteSpace(trim)) continue;

                    var saida = DescobrirAcao(trim);
                    if (!string.IsNullOrEmpty(saida)) respostas.Add(saida);
                }

                return respostas.Count != 0 ? 
                    string.Join(Environment.NewLine, respostas) : 
                    MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO;
            });
        }

        private string DescobrirAcao(string linha) => linha switch
        {
            var l when GalacticParser.MapeamentoNumeroRomano().IsMatch(l) => TratarSimbolo(l),
            var l when GalacticParser.MapeamentoMetal().IsMatch(l) => TratarMetal(l),
            var l when GalacticParser.QuantoE().IsMatch(l) => TratarQuantoE(l),
            var l when GalacticParser.QuantosCreditosE().IsMatch(l) => TratarQuantosCreditosE(l),
            _ => MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO
        };

        private string TratarSimbolo(string linha)
        {
            var match = GalacticParser.MapeamentoNumeroRomano().Match(linha);
            _vocabulario[match.Groups[1].Value] = match.Groups[2].Value.ToUpper();
            return string.Empty;
        }

        private string TratarMetal(string linha)
        {
            var match = GalacticParser.MapeamentoMetal().Match(linha);
            var galatico = match.Groups[1].Value.Split(' ');
            var metal = match.Groups[2].Value;
            var totalCreditos = decimal.Parse(match.Groups[3].Value);

            var romano = string.Concat(galatico.Select(s => _vocabulario.GetValueOrDefault(s, "")));

            if (ConversorNumeroRomanos.TransformarEmNumeroArabico(romano, out int qtd) && qtd > 0)
            {
                _precosMetais[metal] = totalCreditos / qtd;
            }

            return string.Empty;
        }

        private string TratarQuantoE(string linha)
        {
            var match = GalacticParser.QuantoE().Match(linha);
            var conteudo = match.Groups[1].Value.Trim();
            var palavras = conteudo.Split(' ');

            var romano = string.Concat(palavras.Select(p => _vocabulario.GetValueOrDefault(p, "")));

            if (!ConversorNumeroRomanos.TransformarEmNumeroArabico(romano, out int valor))
                return MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO;

            return $"{conteudo} é {valor}";
        }

        private string TratarQuantosCreditosE(string linha)
        {
            var match = GalacticParser.QuantosCreditosE().Match(linha);
            var conteudo = match.Groups[1].Value.Trim();
            var palavras = conteudo.Split(' ');

            var metal = palavras[^1];
            var galatico = palavras[..^1];

            var romano = string.Concat(galatico.Select(s => _vocabulario.GetValueOrDefault(s, "")));

            if (!ConversorNumeroRomanos.TransformarEmNumeroArabico(romano, out int qtd) || 
                !_precosMetais.TryGetValue(metal, out decimal value))
                return MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO;

            var total = qtd * value;
            return $"{string.Join(" ", galatico)} {metal} é {total:G29} Créditos";
        }
    }
}