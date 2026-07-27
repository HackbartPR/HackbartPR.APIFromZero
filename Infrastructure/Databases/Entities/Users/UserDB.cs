using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Databases.Entities.Users
{
    /// <summary>
    /// Representa o usuário no banco de dados.
    /// Essa classe não deve carregar regras de negócio, isso é papel da camada Domain.
    /// 
    /// Devido a classe IdentityUser não possuir campos relacionados a 'Data de Criação' e nem 'Data Atualização', foi necessário criar essa classe UserDB.
    /// </summary>
    public class UserDB : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
