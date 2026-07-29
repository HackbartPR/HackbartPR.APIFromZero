using Microsoft.Extensions.DependencyInjection;
using Migrator.Services.Migration;
using Migrator.Services.Seeders;

namespace Migrator.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para ornização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para injeção de dependência.
    /// DI => Dependency Injector
    /// </summary>
    public static class DIService
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<MigrationService>();
            services.AddScoped<UserSeederService>();

            return services;
        }
    }
}
