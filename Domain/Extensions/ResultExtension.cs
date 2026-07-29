using Domain.Exceptions.Base;
using FluentResults;
using System.Net;
using System.Text;

namespace Domain.Extensions
{
    /// <summary>
    /// Essa extensão servirá como um atalho para podermos ler o HttpStatusCode, Mensagens, ... dos erros que estarão salvos no objeto Result do Result Pattern.
    /// </summary>
    public static class ResultExtension
    {
        /// <summary>
        /// Obtém o código de status HTTP associado ao primeiro erro da operação.
        /// Caso nenhum erro possua um código de status definido, retorna o valor de fallback informado.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static HttpStatusCode GetStatusCode(this ResultBase result, HttpStatusCode fallback = HttpStatusCode.InternalServerError)
            => result.Errors.OfType<BaseError>().FirstOrDefault()?.StatusCode ?? fallback;

        /// <summary>
        /// Converte a coleção de erros do resultado em uma coleção de mensagens de erro.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetErrors(this ResultBase result)
            => [.. result.Errors.Select(e => e.Message)];

        /// <summary>
        /// Concatena todas as mensagens de erro presentes no resultado em uma única string,
        /// separando cada mensagem por uma quebra de linha.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string GetStringErrors(this ResultBase result)
        {
            StringBuilder sb = new();

            foreach (var error in result.Errors)
            {
                sb.AppendLine(error.Message);
            }

            return sb.ToString();
        }
    }
}
