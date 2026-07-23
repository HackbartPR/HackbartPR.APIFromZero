namespace API.Constants
{
    /// <summary>
    /// Classe responsável por amazenar constants que serão utilizadas no context da API
    /// </summary>
    public static class GeneralConstants
    {
        /// <summary>
        /// Nome do Header que deve ser utilizado nas requisições
        /// </summary>
        public const string HeaderIdempotencyName = "X-Idempotency-Key";
    }
}
