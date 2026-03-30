using MerchantsGuideToTheGalaxy.Domain;
using MerchantsGuideToTheGalaxy.Domain.Interfaces;
using MerchantsGuideToTheGalaxy.Service;
using Xunit.Sdk;

namespace MerchantsGuideToTheGalaxy.Tests.UnitTests.Services
{
    public class ConversorGalaticoTestes
    {
        public class GalaxyProcessorTests
        {
            private readonly ConversorGalaticoService _service;

            public GalaxyProcessorTests()
            {
                _service = new ConversorGalaticoService();
            }

            [Fact]
            public async Task DeveProcessarFluxoCompletoDoEnunciado()
            {
                var entrada = """
                                glob é I
                                prok é V
                                pish é X
                                tegj é L
                                glob glob Prata é 34 Créditos
                                glob prok Ouro é 57800 Créditos
                                pish pish Ferro é 3910 Créditos
                                quanto é pish tegj glob glob ?
                                quantos Créditos é glob prok Prata ?
                                quantos Créditos é glob prok Ouro ?
                                quantos Créditos é glob prok Ferro ?
                                quanto de madeira uma marmota poderia roer se uma marmota pudesse roer madeira ?
                                """;

                var esperado = """
                                pish tegj glob glob é 42
                                glob prok Prata é 68 Créditos
                                glob prok Ouro é 57800 Créditos
                                glob prok Ferro é 782 Créditos
                                Não tenho a menor ideia do que você está falando
                                """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);

                Assert.Equal(esperado, resultado);
            }

            [Fact]
            public async Task DeveTratarAtribuicaoDeNovosSimbolosDinamicamente()
            {
                var entrada = """
                              muad é X
                              tegj é I
                              quanto é muad tegj tegj ?
                              """;

                var esperado = "muad tegj tegj é 12";

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(esperado, resultado.Trim());
            }


            [Fact]
            public async Task DeveRetornarErroParaPerguntaTotalmenteInvalida()
            {
                var entrada = "quanto vale o show ?";

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }

            [Fact]
            public async Task DeveValidarRegraDeRepeticaoDosSimbolosRomanos()
            {
                var entrada = """
                              glob é I
                              quanto é glob glob glob glob ?
                              """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Contains(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado);
            }

            [Fact]
            public async Task DeveProcessarConsultasSimultaneasDeTiposDiferentes()
            {
                var entrada = """
                              pish é X
                              tegj é L
                              quanto é pish tegj ?
                              quanto é tegj pish ?
                              """;

                var esperado = """
                               pish tegj é 40
                               tegj pish é 60
                               """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(esperado, resultado);
            }

            [Fact]
            public async Task DeveProcessarSimboloNovoEItemMuad()
            {
                var entrada = """
                              muad é X
                              muad muad Beskar é 4000 Créditos
                              quanto é muad muad ?
                              quantos Créditos é muad Beskar ?
                              """;

                var esperado = """
                               muad muad é 20
                               muad Beskar é 2000 Créditos
                               """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(esperado, resultado);
            }

            [Fact]
            public async Task DeveTratarSubtraçãoRomanaNoContextoGalatico()
            {
                var entrada = """
                              glob é I
                              prok é V
                              pish é X
                              quanto é glob prok ?
                              quanto é glob pish ?
                              """;

                var esperado = """
                               glob prok é 4
                               glob pish é 9
                               """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(esperado, resultado);
            }

            [Fact]
            public async Task DevePerguntaSobreAssuntoAleatorio()
            {
                var entrada = "quanto de madeira uma marmota poderia roer ?";

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }

            [Fact]
            public async Task DeveTentarUsarPishSemAntesDizerOQueEleE()
            {
                var entrada = "quanto é pish ?";

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }

            [Fact]
            public async Task DeveTestarMetalNaoRegistrado()
            {
                var entrada = """
                              glob é I
                              quantos Créditos é glob Beskar ?
                              """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }

            [Fact]
            public async Task DeveVerificarViolacaoDasRegrasRomanasRepeticaoExcessiva()
            {
                var entrada = """
                              glob é I
                              quanto é glob glob glob glob ?
                              """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }

            [Fact]
            public async Task DeveOcorrerErroDeViolacaoDasRegrasRomanasSubtracaoInvalida()
            {
                var entrada = """
                              glob é I
                              prok é V
                              pish é X
                              quanto é prok pish ?
                              """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }

            [Fact]
            public async Task DeveVerificarEntradaComFormatoIncorreto()
            {
                var entrada = "glob é um número legal";

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }

            [Fact]
            public async Task DeveVerificarAQuantidadeInvalidaParaMetal()
            {
                var entrada = """
                              glob é I
                              pish é X
                              pish pish pish pish Beskar é 100 Créditos
                              """;

                var consulta = "quantos Créditos é glob Beskar ?";
                var resultado = await _service.ProcessarConversaoAsync(entrada + "\n" + consulta);

                Assert.Contains(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado);
            }

            [Fact]
            public async Task DeveIgnorarUnidadeDesconhecidaNoMeioDaFrase()
            {
                var entrada = """
                              pish é X
                              quanto é pish krypton pish ?
                              """;

                var esperado = "pish krypton pish é 20";

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(esperado, resultado.Trim());
            }

            [Fact]
            public async Task DeveVerificarViolacaoDeSubtracaoProibida50Menos5()
            {
                var entrada = """
                              prok é V
                              tegj é L
                              quanto é prok tegj ?
                              """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }


            [Fact]
            public async Task DeveConsultaDePrecoAntesDaDefinicao()
            {
                var entrada = """
                              muad é X
                              quantos Créditos é muad Beskar ?
                              """;

                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }

            [Fact]
            public async Task DeveTratarTrocaDeCaixaCaixaAltaEBaixaNasPalavras()
            {
                var entrada = """
                              glob é I
                              quanto é GLOB ?
                              """;


                var resultado = await _service.ProcessarConversaoAsync(entrada);
                Assert.Equal(MensagensDoSistema.NAO_TENHO_IDEIA_DO_QUE_ESTA_FALANDO, resultado.Trim());
            }


        }
    }
}