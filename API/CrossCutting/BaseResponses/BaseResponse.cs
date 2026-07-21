using System.Net;
using System.Text.Json.Serialization;

namespace API.CrossCutting.BaseResponses
{
    /// <summary>
    /// Classe base responsável por padronizar as respostas retornadas pelo sistema,
    /// contendo informações sobre a requisição, mensagem de retorno e status da operação.
    /// </summary>
    public abstract class BaseResponse
    {
        /// <summary>
        /// Armazenará o 'TraceIdentifier' da requisição.
        /// </summary>
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem genérica que ajudará o entendimento do retorno da requisição.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Útil para identificar qual HTTP Code retornar como resposta da requisição.
        /// Devido ao JsonIgnore, essa propriedade não será retornada ao consumidor da API.
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Identificação rápida se a requisição foi atendida com sucesso.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Em casos de falhar, teremos uma lista de erros onde os consumidores da API poderão mostrar aos seus usuários.
        /// </summary>
        public IEnumerable<string> Errors { get; set; } = [];
    }

    /// <summary>
    /// Classe de resposta genérica utilizada para retornar dados junto às informações padrão da resposta.
    /// </summary>
    /// <typeparam name="TData">
    /// Tipo do dado retornado na resposta.
    /// </typeparam>
    public sealed class BaseResponse<TData> : BaseResponse
    {
        /// <summary>
        /// Objeto de retorno. Ex: User (Object), Name (string), Vazio (Null) ...
        /// </summary>
        public TData? Data { get; set; }
    }
}
