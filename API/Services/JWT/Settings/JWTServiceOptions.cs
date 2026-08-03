namespace API.Services.JWT.Settings
{
    /// <summary>
    /// Representa as configurações do JWT Token, mapeadas a partir do arquivo do .json
    /// </summary>
    public sealed record JWTOptions
    {
        /// <summary>
        /// Chave de Identificação.
        /// </summary>
        public const string Identifier = "Services:JWT";

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public string ExpirationInMinutes { get; set; } = string.Empty;

        public string RefreshExpirationInDays { get; set; } = string.Empty;
    }
}
