using API.CrossCutting.BaseResponses;
using System.Net;

namespace API.Middlewares.Exceptions
{
    /// <summary>
    /// Documentação: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-10.0
    /// Middleware responsável por tratar exceções não previstas ou não tratadas pela aplicação.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        /// <summary>
        /// Propriedade obrigatória, responsável por chamar o próximo middleware na pipeline.
        /// </summary>
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Responsável por registrar logs.
        /// </summary>
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        /// <summary>
        /// Método obrigatório para um middleware.
        /// Responsável por conter a lógica/regra de execução do middleware.
        /// 
        /// Lógica => 
        /// Este middleware será o primeiro da pipeline, ou seja, toda a aplicação passará por ele.
        /// Dessa forma, ao colocar um try-catch, será possível captar qualquer erro não tratado em qualquer etapa do ciclo de vida de uma requisição.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante a execução {Context}", context.Request.Path.Value);

                var response = new BaseResponse<string>
                {
                    Success = false,
                    RequestId = context.TraceIdentifier,
                    Errors = new List<string> { ex.Message },
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Ocorreu um erro inesperado. Tente novamente mais tarde."
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
