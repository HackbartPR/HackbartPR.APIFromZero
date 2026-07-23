using Infrastructure.Services.Cache.Enums;
using Infrastructure.Services.Cache.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Infrastructure.Services.Cache.Redis
{
    /// <summary>
    /// Tipo Serviço: Singleton
    /// Documentação: https://stackexchange.github.io/StackExchange.Redis
    /// Classe responsável por conversar diretamente com o SDK StackExchangeRedis
    /// </summary>
    public sealed class RedisService : ICacheService
    {
        private IConnectionMultiplexer _connection;

        /// <summary>
        /// A conexão com o Redis será aberta via construtor e permanecerá aberta para todas as conexões, devido a ser totalmente thread-safe entre operações.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RedisService(IOptions<CacheServiceOptions> options)
        {
            CacheServiceOptions _options = options.Value ?? throw new ArgumentNullException(nameof(options), "Não inicializado");

            ConfigurationOptions configurationOptions = ConfigurationOptions.Parse(_options.ConnectionString);
            configurationOptions.ReconnectRetryPolicy = new ExponentialRetry(5000);
            configurationOptions.ConnectRetry = 5;

            _connection = ConnectionMultiplexer.Connect(configurationOptions);
        }

        /// <summary>
        /// Responsável por salvar um par de chave-valor. 
        /// </summary>
        /// <param name="key">Chave</param>
        /// <param name="value">Valor</param>
        /// <param name="ttl">Tempo para expiração</param>
        /// <param name="when">Condição para registrar a chave</param>
        /// <returns></returns>
        public async Task<bool> SetKeyAsync(string key, string value, TimeSpan? ttl = null, CacheWhen? when = CacheWhen.Always)
        {
            When whenRedis = when switch
            {
                CacheWhen.NotExists => When.NotExists,
                CacheWhen.Exists => When.Exists,
                _ => When.Always
            };

            IDatabase db = _connection.GetDatabase();
            return await db.StringSetAsync(key, value, ttl, whenRedis);
        }

        public void Dispose()
        {
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
