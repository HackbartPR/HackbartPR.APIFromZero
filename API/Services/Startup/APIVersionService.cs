using Asp.Versioning;

namespace API.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para ornização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para versionamento da API.
    /// </summary>
    public static class APIVersionService
    {
        /// <summary>
        /// Extension do IServiceCollection responsável por criar o versionamento da API.
        /// Como exemplo, foram criadas duas versões, sendo a V1 a default.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAPIVersionService(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddOpenApi("v1");
            services.AddOpenApi("v2");

            return services;
        }
    }
}
