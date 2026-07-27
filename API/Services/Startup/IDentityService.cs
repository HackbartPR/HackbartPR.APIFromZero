using Infrastructure.Databases.Contexts;
using Infrastructure.Databases.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para organização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para a biblioteca IDentity.
    /// </summary>
    public static class IDentityService
    {
        /// <summary>
        /// MÉTODO INCOMPLETO (SERÁ ALTERADO ASSIM QUE ENTRARMOS NO POST DO IDENTITY)
        /// Até o momento, essa extensão serve para configurar o IDentity na nossa pipeline e configurar qual DBContext será utilizado para armazenar a estrutura do IDentity.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<UserDB, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<EFContext>();

            return services;
        }
    }
}
