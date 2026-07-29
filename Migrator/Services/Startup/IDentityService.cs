using Infrastructure.Databases.Contexts;
using Infrastructure.Databases.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Migrator.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para organização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para a biblioteca IDentity.
    /// </summary>
    public static class IDentityService
    {
        /// <summary>
        /// Até o momento, essa extensão serve para configurar o IDentity na nossa pipeline e configurar qual DBContext será utilizado para armazenar a estrutura do IDentity.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            string lockoutTime = configuration["Login:LockoutTimeInMinutes"] ?? "";
            long lockoutTimeParsed = long.TryParse(lockoutTime, out long parsed) ? parsed : 5;

            services.AddIdentity<UserDB, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutTimeParsed);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789áãéíóúõ-._@#+ ";
            })
            .AddEntityFrameworkStores<EFContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
