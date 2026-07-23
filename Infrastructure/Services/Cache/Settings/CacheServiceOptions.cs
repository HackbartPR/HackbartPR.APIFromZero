namespace Infrastructure.Services.Cache.Settings
{
    /// <summary>
    /// Documentação: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-10.0
    /// Classe responsável por representar as variáveis de ambiente no appSettings.* referentes ao serviços de cache.
    /// </summary>
    public sealed record CacheServiceOptions
    {
        /// <summary>
        /// Chave de Identificação.
        /// </summary>
        public const string Identifier = "Services:Cache";

        public string ConnectionString { get; set; } = string.Empty;
    }
}
