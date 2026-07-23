namespace Infrastructure.Services.Cache.Enums
{
    /// <summary>
    /// Será utilizado para definir quando salvar uma chave no servidor de cache.
    /// 
    /// Ex: NotExists => só salvará uma chave no servidor quando a mesma não existir.
    /// </summary>
    public enum CacheWhen
    {
        Always,

        Exists,

        NotExists
    }
}
