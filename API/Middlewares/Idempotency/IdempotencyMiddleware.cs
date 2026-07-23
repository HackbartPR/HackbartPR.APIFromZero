using API.Constants;
using API.CrossCutting.BaseResponses;
using API.Middlewares.Idempotency.Settings;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Cache.Enums;
using Microsoft.Extensions.Options;
using System.Net;

namespace API.Middlewares.Idempotency
{
    /// <summary>
    /// Documentação: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-10.0
    /// Middleware resposável por garantir que cada requisição seja única através do Header X-Idempotency-Key.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="cache"></param>
    /// <param name="options"></param>
    public class IdempotencyMiddleware(RequestDelegate next, ICacheService cache, IOptions<IdempotencyOptions> options)
    {
        private readonly ICacheService _cache = cache;
        private readonly RequestDelegate _next = next;
        private readonly IdempotencyOptions _options = options.Value;

        /// <summary>
        /// Método chamado pela Pipeline, nele que colocamos todas as regras do nosso middleware.
        /// A idempotência ocorrerá somente nas requisições que alteram o estado de alguma entidade ('POST', 'PUT', 'PATCH').
        /// A chave Idempotency-Key só será salva no servidor de cache, caso ela não exista nele.
        ///
        /// Lógica da cache no servidor de cache:
        /// Caso a Idempotency-Key já exista no servidor de cache, indica que a requisição já ocorreu anteriormente, ou seja, houve duplicação.
        /// Caso a Idempotency-Key não exista no servidor, indica que é a primeira vez que essa requisição é feita, portanto será salva no cache e permitiremos o 
        /// request seguir seu caminho para os outros middlewares.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            string[] methodsToCheck = ["POST", "PUT", "PATCH"];
            if (!methodsToCheck.Contains(context.Request.Method))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(GeneralConstants.HeaderIdempotencyName, out var idempotencyKey))
            {
                var response = new BaseResponse<string>
                {
                    Success = false,
                    RequestId = context.TraceIdentifier,
                    Errors = new List<string> { "Idempotency-Key é obrigatório" },
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Header não encontrado."
                };

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(response);
                return;
            }

            int ttl = int.Parse(_options.TTLInMinutes);
            bool saved = await _cache.SetKeyAsync(idempotencyKey.ToString(), "", TimeSpan.FromMinutes(ttl), CacheWhen.NotExists);
            if (!saved)
            {
                var response = new BaseResponse<string>
                {
                    Success = false,
                    RequestId = context.TraceIdentifier,
                    Errors = new List<string> { "Requisição já processada" },
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ""
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(response);
                return;
            }

            await _next(context);
        }
    }
}
