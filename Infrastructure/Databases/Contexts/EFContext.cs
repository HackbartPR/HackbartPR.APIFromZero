using Infrastructure.Databases.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases.Contexts
{
    /// <summary>
    /// Tipo Serviço: Scoped
    /// Documentação: https://learn.microsoft.com/pt-br/ef/core/
    /// Documentação: https://learn.microsoft.com/pt-br/ef/core/dbcontext-configuration/?source=recommendations
    /// Documentação: https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=visual-studio
    /// Classe para configuração do ORM Entity Framework Core, ela deve ser a representação do banco de dados aqui no servidor.
    /// 
    /// Como utilizaremos a estrutura de tabelas do IDentity para gerenciar usuários da aplicação, foi necessário trocar de 'DbContext' para 'IdentityDbContext'.
    /// O Guid passado como 'Type' em 'IdentityDbContext' será necessário para podermos dizer ao IDentity que o ID será um Guid.
    /// </summary>
    /// <param name="options"></param>
    public class EFContext(DbContextOptions<EFContext> options) : IdentityDbContext<UserDB, IdentityRole<Guid>, Guid>(options)
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
