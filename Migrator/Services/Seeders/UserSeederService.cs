using Domain.Constants;
using Domain.Entities.Users;
using Domain.Extensions;
using Domain.ValueObjects;
using FluentResults;
using Infrastructure.Databases.Contexts;
using Infrastructure.Databases.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Migrator.Services.Seeders.Settings;

namespace Migrator.Services.Seeders
{
    /// <summary>
    /// Responsável por criar e manter os dados iniciais relacionados a usuários e perfis de acesso.
    /// Executa a inserção das informações obrigatórias para que a aplicação seja iniciada e utilizada corretamente em um ambiente recém-criado.
    /// 
    /// Apesar de estarmos utilizando Result Pattern, este serviço deve rodar na pipeline e deve obrigatóriamente gerar um erro caso alguma regra seja infligida. 
    /// Portanto, lançaremos Exceptions
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="seedOptions"></param>
    /// <param name="context"></param>
    /// <param name="roleManager"></param>
    /// <param name="userManager"></param>
    /// <param name="userStore"></param>
    public sealed class UserSeederService(ILogger<UserSeederService> logger, IOptions<UserSeederOptions> seedOptions, EFContext context, RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<UserDB> userManager, IUserStore<UserDB> userStore)
    {
        private readonly ILogger<UserSeederService> _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Serviço não inicializado.");
        private readonly UserSeederOptions _seedOptions = seedOptions.Value ?? throw new ArgumentNullException(nameof(seedOptions), "Serviço não inicializado.");
        private readonly EFContext _context = context ?? throw new ArgumentNullException(nameof(context), "Serviço não inicializado.");
        private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager), "Serviço não inicializado.");
        private readonly UserManager<UserDB> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "Serviço não inicializado.");
        private readonly IUserStore<UserDB> _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore), "Serviço não inicializado.");

        /// <summary>
        /// Método Main do processo de inserção das Seeds
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Iniciando UserSeeder Service");

                await InitializeUserRoles();
                await InitializeUserAdminSeeder(cancellationToken);
                await InitializeUserCustomerSeeder(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante a execução do UserSeederService");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Como estamos utilizando o IDentity, vamos utilizar o sistema de Roles que temos disponível, portanto antes de inserir o usuário, 
        /// vamos dizer quais Roles já devem nascer junto com o sistema.
        /// </summary>
        /// <returns></returns>
        private async Task InitializeUserRoles()
        {
            var roleAdm = await _roleManager.RoleExistsAsync(RoleConstants.Admin);
            var roleCus = await _roleManager.RoleExistsAsync(RoleConstants.Customer);

            if (!roleAdm)
            {
                _logger.LogInformation("Alimentando base de dados com role: {Role}.", RoleConstants.Admin);

                await _roleManager.CreateAsync(new IdentityRole<Guid>()
                {
                    Name = RoleConstants.Admin,
                    NormalizedName = RoleConstants.AdminNormalized,
                });
            }

            if (!roleCus)
            {
                _logger.LogInformation("Alimentando base de dados com role: {User}.", RoleConstants.Customer);

                await _roleManager.CreateAsync(new IdentityRole<Guid>()
                {
                    Name = RoleConstants.Customer,
                    NormalizedName = RoleConstants.CustomerNormalized,
                });
            }
        }

        /// <summary>
        /// Responsável por inserir um Usuário Admin no sistema
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task InitializeUserAdminSeeder(CancellationToken cancellationToken)
        {
            if (_seedOptions.UserAdminSeed == null || string.IsNullOrEmpty(_seedOptions.UserAdminSeed.UserName) || string.IsNullOrEmpty(_seedOptions.UserAdminSeed.Email))
                throw new Exception("Seed 'Usuário Admin' não foi encontrada");

            if (await _context.Users.AnyAsync(u => !string.IsNullOrEmpty(u.UserName) && u.UserName.ToUpper().Equals(_seedOptions!.UserAdminSeed.UserName.ToUpper()), cancellationToken))
                return;

            _logger.LogInformation("Alimentando base de dados com usuário: {User}.", _seedOptions.UserAdminSeed.UserName);

            Email email = new();
            Result emailValidation = email.Create(_seedOptions.UserAdminSeed.Email);

            if (emailValidation.IsFailed)
                throw new Exception(emailValidation.GetStringErrors());

            User user = new();
            Result userValidation = user.Create(_seedOptions.UserAdminSeed.UserName, email);

            if (userValidation.IsFailed)
                throw new Exception(userValidation.GetStringErrors());

            UserDB newUser = new()
            {
                Id = user.Id,
                UserName = _seedOptions.UserAdminSeed.UserName,
                EmailConfirmed = true,
                Email = _seedOptions.UserAdminSeed.Email,
                CreatedAt = DateTime.UtcNow
            };

            IUserEmailStore<UserDB> emailStore = (IUserEmailStore<UserDB>)_userStore;

            await _userStore.SetUserNameAsync(newUser, user.Name, cancellationToken);
            await emailStore.SetEmailAsync(newUser, newUser.Email, cancellationToken);

            await _userManager.CreateAsync(newUser, _seedOptions.UserAdminSeed.Password);
            await _userManager.AddToRoleAsync(newUser, RoleConstants.Admin);
        }

        /// <summary>
        /// Responsável por inserir um usuário normal no sistema.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task InitializeUserCustomerSeeder(CancellationToken cancellationToken)
        {
            if (_seedOptions.UserCustomerSeed == null || string.IsNullOrEmpty(_seedOptions.UserCustomerSeed.UserName) || string.IsNullOrEmpty(_seedOptions.UserCustomerSeed.Email))
                throw new Exception("Seed 'Usuário Cliente' não foi encontrada");

            if (await _context.Users.AnyAsync(u => !string.IsNullOrEmpty(u.UserName) && u.UserName.ToUpper().Equals(_seedOptions!.UserCustomerSeed.UserName.ToUpper()), cancellationToken))
                return;

            _logger.LogInformation("Alimentando base de dados com usuário: {User}.", _seedOptions.UserCustomerSeed.UserName);

            Email email = new();
            Result emailValidation = email.Create(_seedOptions.UserCustomerSeed.Email);

            if (emailValidation.IsFailed)
                throw new Exception(emailValidation.GetStringErrors());

            User user = new();
            Result userValidation = user.Create(_seedOptions.UserCustomerSeed.UserName, email);

            if (userValidation.IsFailed)
                throw new Exception(userValidation.GetStringErrors());

            UserDB newUser = new()
            {
                Id = user.Id,
                UserName = _seedOptions.UserCustomerSeed.UserName,
                EmailConfirmed = true,
                Email = _seedOptions.UserCustomerSeed.Email,
                CreatedAt = DateTime.UtcNow
            };

            IUserEmailStore<UserDB> emailStore = (IUserEmailStore<UserDB>)_userStore;

            await _userStore.SetUserNameAsync(newUser, user.Name, cancellationToken);
            await emailStore.SetEmailAsync(newUser, newUser.Email, cancellationToken);

            await _userManager.CreateAsync(newUser, _seedOptions.UserCustomerSeed.Password);
            await _userManager.AddToRoleAsync(newUser, RoleConstants.Customer);
        }
    }
}
