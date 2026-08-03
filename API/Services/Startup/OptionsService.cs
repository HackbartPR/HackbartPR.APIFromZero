using API.Middlewares.Idempotency.Settings;
using API.Services.JWT.Settings;
using Infrastructure.Services.Cache.Settings;

namespace API.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para ornização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para aplicar o padrão IOptions.
    /// </summary>
    public static class OptionsService
    {
        public static IServiceCollection AddOptionsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheServiceOptions>(configuration.GetSection(CacheServiceOptions.Identifier));
            services.Configure<IdempotencyOptions>(configuration.GetSection(IdempotencyOptions.Identifier));
            services.Configure<JWTOptions>(configuration.GetSection(JWTOptions.Identifier));

            return services;
        }
    }
}
