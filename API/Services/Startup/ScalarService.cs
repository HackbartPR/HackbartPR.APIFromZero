using Scalar.AspNetCore;

namespace API.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para ornização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para o Scalar.
    /// Para acessar a Documentação criada pelo Scalar, 
    /// </summary>
    public static class ScalarService
    {
        /// <summary>
        /// Extension do IServiceCollection responsável por configurar o Scalar.
        /// Ao criar a API com SDK 'Microsoft.NET.Sdk.Web' já temos a opção de trabalhar com o padrão OpenAPI. Aqui estamos configurando uma rota para a documentação .json.
        /// Ao mesmo tempo, estamos configurando o UI e a features do Scalar.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication AddDocumentionService(this WebApplication app)
        {
            app.MapOpenApi("api/docs/{documentName}/openapi.json");

            app.MapScalarApiReference("api/docs", options =>
            {
                options.OpenApiRoutePattern = "api/docs/{documentName}/openapi.json";
                options.Title = "API From Zero - Documentação";
                options.Layout = ScalarLayout.Classic;
                options.Theme = ScalarTheme.Kepler;
                options.DarkMode = true;
            });

            return app;
        }
    }
}
