using Domain.Entities.Base;
using Domain.Exceptions;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Entities.Users
{
    /// <summary>
    /// Representa um usuário no domínio da aplicação.
    /// Essa classe deve possuir toda regra de negócio vinculada ao usuário, tendo como um dos princípios do DDD.
    /// </summary>
    public sealed class User() : BaseEntity()
    {
        public string Name { get; private set; } = string.Empty;

        public Email Email { get; private set; } = null!;

        public DateTime CreatedAt { get; private set; }

        public DateTime? UpdateAt { get; private set; }

        public Result Create(string name, Email email)
        {
            Name = name;
            Email = email;

            return Validate();
        }

        protected override Result Validate()
        {
            Result result = new();

            if (string.IsNullOrWhiteSpace(Name))
                result.WithError(DomainError.EmptyName);

            if (Name.Length < 3)
                result.WithError(DomainError.InvalidName);

            return result;
        }
    }
}
