using API.Services.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.LowercaseUrls = true; }); // Deixa as rotas/paths dos endpoints todas minúsculas.
builder.Services.AddControllers(); // Já vem configurado no Program.cs
builder.Services.AddDependencies(); // Extensão criada para configurar o injetor de dependência
builder.Services.AddAPIVersionService(); // Extensão criada para versionamento da API
builder.Services.AddEFDatabaseService(builder.Configuration); // Extensão para configurar conexão com o banco de dados com ORM EF
builder.Services.AddOptionsService(builder.Configuration); // Extensão criada para configurar todos os OptionsServices
builder.Services.AddHealthCheckService(); // Serviço de HealthCheck criado.

var app = builder.Build();

app.UseErrorHandler(); //Middleware de tratamento de exceptions

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.AddDocumentionService(); //Adiciona documentação do OpenAPI e Scalar.
}

app.UseHttpsRedirection();

app.UseIdempotencyHandler(); //Middleware de Idempotencia.

app.UseAuthorization();

app.MapControllers();

app.SetHealthChecks(); //Chamado quando algum serviço chama o endpoint configurado dentro dessa extension

app.Run();
