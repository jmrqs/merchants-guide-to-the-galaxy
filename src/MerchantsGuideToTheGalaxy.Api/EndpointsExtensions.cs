using MerchantsGuideToTheGalaxy.Domain;
using MerchantsGuideToTheGalaxy.Domain.Interfaces;
using MerchantsGuideToTheGalaxy.Domain.Models.Request;
using MerchantsGuideToTheGalaxy.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace MerchantsGuideToTheGalaxy.Api
{
    public static class EndpointsExtensions
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/conversor")
                           .WithTags("Conversor Elementos em Créditos");

            group.MapPost("/", async (
                [FromBody] ConversorRequest request,
                IConversorGalaticoService conversor) =>
            {
                if (string.IsNullOrWhiteSpace(request.DadosDeEntrada))
                    return Results.BadRequest(new { message = MensagensDoSistema.A_ENTRADA_NAO_PODE_SER_VAZIA });

                try
                {
                    var response = await conversor.ProcessarConversaoAsync(request.DadosDeEntrada);
                    return Results.Ok(response);
                }
                catch (Exception)
                {
                    return Results.Problem(MensagensDoSistema.ERROR_AO_CONVERTER_ELEMENTOS_EM_CREDITOS);
                }
            })
            .WithName("ConversorElementosEmCreditos")
            .Produces<ConversorResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
