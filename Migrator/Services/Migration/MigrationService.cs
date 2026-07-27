using Infrastructure.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Migrator.Services.Migration
{
    /// <summary>
    /// Class/Service responsável por verificar se existe alguma migration criada que ainda não foi aplicada a base de dados. Em caso positivo, executará as migrations.
    /// Este serviço deverá ter seu ciclo de vida curto, ou seja, deverá ser executado uma única vez quando o projeto Migrator for executado.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    public sealed class MigrationService(ILogger<MigrationService> logger, EFContext context)
    {
        private readonly ILogger<MigrationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Serviço não inicializado.");
        private readonly EFContext _context = context ?? throw new ArgumentNullException(nameof(logger), "Serviço não inicializado.");

        public async Task RunAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando Migration Service");

                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante a execução do MigrationService");
                Environment.Exit(1);
            }
        }
    }
}
