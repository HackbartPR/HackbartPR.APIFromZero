using API.CrossCutting.BaseResponses;
using Microsoft.AspNetCore.Mvc;

namespace API.CrossCutting.BaseControllers
{
    /// <summary>
    /// Todos os controller criados deverão herdar essa classe.
    /// Ela ajudará na não duplicação de código.
    /// </summary>
    /// <param name="logger"></param>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseAPIController(ILogger logger) : ControllerBase
    {
        /// <summary>
        /// Variável não está sendo utilizada neste exemplo, mas serve para registrar logs.
        /// </summary>
        protected readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(ILogger), "Serviço não inicializado.");

        /// <summary>
        /// Método utilizado padronizar as respostas enviadas pelos controllers.
        /// </summary>
        /// <param name="response">Classe Padrão para as respostas da API</param>
        /// <returns></returns>
        protected IActionResult ApplicationResponse(BaseResponse response)
        {
            response.RequestId = HttpContext.TraceIdentifier;

            if (response.Success)
                return Ok(response);

            return response.StatusCode.HasValue
                ? StatusCode((int)response.StatusCode, response)
                : StatusCode(500, response);
        }
    }
}
