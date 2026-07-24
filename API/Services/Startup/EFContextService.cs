using Infrastructure.Databases.Contexts;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para ornização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para configurar Context do EF.
    /// </summary>
    public static class EFContextService
    {
        public static IServiceCollection AddEFDatabaseService(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["Services:Database:ConnectionString"];
            services.AddDbContext<EFContext>(options =>
            {
                options.UseSqlServer(connectionString ?? "");
            });

            return services;
        }
    }
}
