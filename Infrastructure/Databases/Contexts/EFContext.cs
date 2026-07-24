using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases.Contexts
{
    /// <summary>
    /// Tipo Serviço: Scoped
    /// Documentação: https://learn.microsoft.com/pt-br/ef/core/
    /// Documentação: https://learn.microsoft.com/pt-br/ef/core/dbcontext-configuration/?source=recommendations
    /// Classe para configuração do ORM Entity Framework Core, ela deve ser a representação do banco de dados aqui no servidor.
    /// 
    /// OBS: Essa classe será alterada quando implementarmos o IDentity
    /// </summary>
    /// <param name="options"></param>
    public class EFContext(DbContextOptions<EFContext> options) : DbContext(options)
    {
        /// <summary>
        /// Método responsável por definir as configurações das tabelas que serão salvas no banco de dados.
        /// No momento, não possui nenhum configuração.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
