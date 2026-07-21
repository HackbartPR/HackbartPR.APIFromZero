using API.Services.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.LowercaseUrls = true; }); // Deixa as rotas/paths dos endpoints todas minúsculas.
builder.Services.AddControllers(); // Já vem configurado no Program.cs
builder.Services.AddAPIVersionService(); // Extensão criada para versionamento da API

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.AddDocumentionService(); //Adiciona documentação do OpenAPI e Scalar.
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
