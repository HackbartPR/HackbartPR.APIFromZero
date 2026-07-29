using FluentResults;
using System.Net;

namespace Domain.Exceptions.Base
{
    /// <summary>
    /// Essa classe extende as características da classe de Error do FluentValidation. Dessa forma, conseguimos incluir um HttpStatusCode em cada erro.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="statusCode"></param>
    public sealed class BaseError(string message, HttpStatusCode statusCode) : Error(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }
}
