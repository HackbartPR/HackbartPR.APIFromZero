namespace API.Services.Authentication.DTOs
{
    /// <summary>
    /// Representa o payload utilizado pelo endpoint /register
    /// </summary>
    public sealed record RegisterRequest
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
