using API.Middlewares.Exceptions;
using API.Middlewares.Idempotency;

namespace API.Services.Startup
{
    /// <summary>
    /// Classe criada apenas para ornização, o mesmo poderia ser feito diretamente no Program.cs.
    /// Responsável por criar extensions do IServiceCollection ou IApplicationBuilder especificamente para os middlewares criados.
    /// </summary>
    public static class MiddlewareService
    {
        /// <summary>
        /// Extensão para Middleware de Exceptions
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }

        /// <summary>
        /// Extensão para o middleware de Idempotencia
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIdempotencyHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IdempotencyMiddleware>();
        }
    }
}
