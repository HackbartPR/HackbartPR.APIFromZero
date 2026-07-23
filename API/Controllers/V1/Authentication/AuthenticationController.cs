using API.CrossCutting.BaseControllers;
using API.CrossCutting.BaseResponses;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.V1.Authentication
{
    /// <summary>
    /// Controller conterá todos os endpoints relacionados a autenticação, registro, passwords ...
    /// </summary>
    /// <param name="logger"></param>
    [ApiVersion("1.0")]
    public class AuthenticationController(ILogger<AuthenticationController> logger) : BaseAPIController(logger)
    {
        /// <summary>
        /// Endpoint utilizado apenas para testes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Teste()
        {
            return ApplicationResponse(new BaseResponse<string>
            {
                Success = true,
                Message = "Teste realizado na Versão 1.0",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Endpoint utilizado apenas simular um erro não tratado.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Exception")]
        public IActionResult TesteException()
        {
            string? strSample = null;

            return ApplicationResponse(new BaseResponse<int>
            {
                Data = strSample.Length,
                Success = true,
                Message = "Teste realizado na Versão 1.0",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Endpoint utilizado apenas teste de idempotência..
        /// </summary>
        /// <returns></returns>
        [HttpPost("Idempotency")]
        public IActionResult TesteIdempotency()
        {
            return ApplicationResponse(new BaseResponse<string>
            {
                Success = true,
                Message = "Requisição recebida com sucesso",
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
