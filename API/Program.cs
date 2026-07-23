using API.Services.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.LowercaseUrls = true; }); // Deixa as rotas/paths dos endpoints todas minúsculas.
builder.Services.AddControllers(); // Já vem configurado no Program.cs
builder.Services.AddDependencies(); // Extensão criada para configurar o injetor de dependência
builder.Services.AddAPIVersionService(); // Extensão criada para versionamento da API
builder.Services.AddOptionsService(builder.Configuration); // Extensão criada para configurar todos os OptionsServices

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

app.Run();
