using MerchantsGuideToTheGalaxy.Api;
using MerchantsGuideToTheGalaxy.Domain.Interfaces;
using MerchantsGuideToTheGalaxy.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IConversorGalaticoService, ConversorGalaticoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
