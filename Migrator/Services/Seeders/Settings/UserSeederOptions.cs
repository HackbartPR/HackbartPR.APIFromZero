using Infrastructure.Databases.Entities.Users;

namespace Migrator.Services.Seeders.Settings
{
    /// <summary>
    /// Representa as configurações das seeds dos usuários, mapeadas a partir do arquivo do .json
    /// </summary>
    public sealed class UserSeederOptions
    {
        /// <summary>
        /// Chave de Identificação principal
        /// </summary>
        public const string Identifier = "Seeds";

        /// <summary>
        /// Teremos uma Seed a qual representará um Admin
        /// </summary>
        public UserOption? UserAdminSeed { get; set; }

        /// <summary>
        /// Teremos uma Seed a qual representará um usuário normal da aplicação
        /// </summary>
        public UserOption? UserCustomerSeed { get; set; }
    }

    /// <summary>
    /// Classe criada separadamente para poder herdar as propriedades da tabela User (UserDB).
    /// </summary>
    public sealed class UserOption : UserDB
    {
        public string Password { get; set; } = string.Empty;
    }
}
