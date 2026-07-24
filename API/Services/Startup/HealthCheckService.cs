using API.CrossCutting.BaseResponses;
using Infrastructure.Databases.Contexts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para organização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para o serviço de Health Check.
    /// </summary>
    public static class HealthCheckService
    {
        /// <summary>
        /// Extension do IServiceCollection responsável por adicionar HealthCheck service na pipeline.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHealthCheckService(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddDbContextCheck<EFContext>();

            return services;
        }

        /// <summary>
        /// Mapeia o endpoint de Health Check (/healthz) e retorna uma resposta JSON
        /// personalizada contendo o status da aplicação.
        /// 
        /// Retorna 200 em caso do servidor e banco de dados estiver funcionando.
        /// Retorna 503 caso um dos dois não esteja funcionando.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication? SetHealthChecks(this WebApplication? app)
        {
            app?.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var isHealthy = report.Status == HealthStatus.Healthy;

                    var data = new HealthCheckData
                    {
                        Status = report.Status.ToString(),
                        TotalDuration = report.TotalDuration,
                        Checks = report.Entries.Select(e => new HealthCheckEntry
                        {
                            Name = e.Key,
                            Status = e.Value.Status.ToString(),
                            Description = e.Value.Description ?? string.Empty,
                            Duration = e.Value.Duration
                        })
                    };

                    BaseResponse<HealthCheckData> response = new()
                    {
                        Data = data,
                        RequestId = context.TraceIdentifier,
                        Success = isHealthy
                    };

                    context.Response.ContentType = "application/json; charset=utf-8";
                    context.Response.StatusCode = isHealthy
                        ? StatusCodes.Status200OK
                        : StatusCodes.Status503ServiceUnavailable;

                    await context.Response.WriteAsJsonAsync(response);
                }
            });

            return app;
        }

        /// <summary>
        /// Classe representa o estado atual do servidor com os serviços conectados.
        /// Nele podemos mostrar quanto tempo está levando a requisição de healthCheck, isso pode ajudar o frontend ou outras ferramentas a verificar 
        /// se continua ou não enviando requisições.
        /// </summary>
        private class HealthCheckData
        {
            public string Status { get; set; } = string.Empty;
            public IEnumerable<HealthCheckEntry> Checks { get; set; } = [];
            public TimeSpan TotalDuration { get; set; }
        }

        /// <summary>
        /// Representa cada verificação realizada.
        /// </summary>
        private class HealthCheckEntry
        {
            public string Name { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public TimeSpan Duration { get; set; }
        }
    }
}
