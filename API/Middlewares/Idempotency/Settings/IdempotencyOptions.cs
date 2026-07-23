namespace API.Middlewares.Idempotency.Settings
{
    /// <summary>
    /// Documentação: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-10.0
    /// Classe responsável por representar as variáveis de ambiente no appSettings.* referentes ao middleware de Idempotencia.
    /// </summary>
    public sealed record IdempotencyOptions
    {
        /// <summary>
        /// Chave de Identificação.
        /// </summary>
        public const string Identifier = "IdempotencyKey";

        public string TTLInMinutes { get; set; } = string.Empty;
    }
}
