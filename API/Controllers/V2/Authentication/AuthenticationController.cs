using API.CrossCutting.BaseControllers;
using API.CrossCutting.BaseResponses;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.V2.Authentication
{
    /// <summary>
    /// Controller conterá todos os endpoints relacionados a autenticação, registro, passwords ...
    /// Este Endpoint da Versão 2.0 só será utilizado para mostrar o funcionamento do versionamento, ou seja, não conterá funcionalidades.
    /// </summary>
    /// <param name="logger"></param>
    [ApiVersion("2.0")]
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
                Message = "Teste realizado na Versão 2.0",
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
