namespace API.Constants
{
    /// <summary>
    /// Classe responsável por amazenar constants que serão utilizadas no context de Autenticação
    /// </summary>
    public static class AuthenticationConstants
    {
        /// <summary>
        /// Nome do token que armazenará o token de autenticação/autorização
        /// </summary>
        public const string TokenCookie = "access_token";

        /// <summary>
        /// Nome do token que armazenará o token de atualização do token de autenticação/autorização
        /// </summary>
        public const string RefreshTokenCookie = "refresh_token";
    }
}
