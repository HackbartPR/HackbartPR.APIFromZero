namespace API.Services.JWT.DTOs
{
    /// <summary>
    /// DTO utilizado pelo AuthenticationService onde conterá os tokens de Autenticação e Renovação
    /// </summary>
    public sealed record TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
