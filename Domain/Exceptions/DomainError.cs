using Domain.Exceptions.Base;
using FluentResults;
using System.Net;

namespace Domain.Exceptions
{
    /// <summary>
    /// Essa classe é um "atalho" e uma lista padronizada de erros que utilizaremos durante as validações do projeto.
    /// Ela evita de ficarmos criando o mesmo erro várias vezes e utilizando mensagens e/ou HttpStatusCode diferentes.
    /// </summary>
    public sealed record DomainError
    {
        public const string StatusCode = nameof(StatusCode);

        public static readonly IError EmptyEmail = new BaseError("O Campo 'Endereco Email' não pode ser nulo ou vazio.", HttpStatusCode.BadRequest);
        public static readonly IError InvalidEmail = new BaseError("O Campo 'Endereco Email' não é válido.", HttpStatusCode.BadRequest);
        public static readonly IError TimeOutEmailValidation = new BaseError("A validação do e-mail demorou mais do que o esperado. Por favor, tente novamente.", HttpStatusCode.GatewayTimeout);

        public static readonly IError EmptyName = new BaseError("O Campo 'Nome' não pode ser nulo ou vazio.", HttpStatusCode.BadRequest);
        public static readonly IError InvalidName = new BaseError("O Campo 'Nome' deve possuir no mínimo 3 caracteres.", HttpStatusCode.BadRequest);
    }
}
