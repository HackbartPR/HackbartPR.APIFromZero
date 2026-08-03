using Domain.Exceptions.Base;
using FluentResults;
using System.Net;

namespace API.Exceptions
{
    /// <summary>
    /// Essa classe é um "atalho" e uma lista padronizada de erros que utilizaremos durante as validações da API.
    /// Ela evita de ficarmos criando o mesmo erro várias vezes e utilizando mensagens e/ou HttpStatusCode diferentes.
    /// </summary>
    public sealed record APIError
    {
        public const string StatusCode = nameof(StatusCode);

        public static readonly IError Unauthorized = new BaseError("O usuário não possui permissão para operação desejada.", HttpStatusCode.Unauthorized);
        public static readonly IError UnauthorizedRefreshToken = new BaseError("Refresh token inválido ou expirado.", HttpStatusCode.Unauthorized);
    }
}
