using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migrator.Services.Migration;
using Migrator.Services.Startup;

/// <summary>
/// Este é um Console.App, ele foi criado para servir como um serviço separado da API.
/// Seu papel será executar migrations criadas na camada Infrastructure.
/// Em produção, este serviço deve entrar na pipeline de de deploy, onde será executado apenas uma vez antes da publicação.
/// </summary>
/// 
var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDependencies();
builder.Services.AddEFDatabaseService(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();

var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
await migrationService.RunAsync();
