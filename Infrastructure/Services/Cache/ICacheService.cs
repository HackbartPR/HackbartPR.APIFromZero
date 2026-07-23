using Infrastructure.Services.Cache.Enums;

namespace Infrastructure.Services.Cache
{
    /// <summary>
    /// Interface responsável por criar um contrato para todos os servidores de cache que venham a ser utilizado.
    /// Essa interface deve ser totalmente desacoplada a qualquer SDK de algum servidor de Cache.
    /// </summary>
    public interface ICacheService : IDisposable
    {
        Task<bool> SetKeyAsync(string key, string value, TimeSpan? ttl = null, CacheWhen? when = CacheWhen.Always);
    }
}
