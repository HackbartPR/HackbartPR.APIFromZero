using Infrastructure.Databases.Entities.Users;

namespace Infrastructure.Databases.Entities.Token
{
    public class RefreshTokenDB
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public string Jti { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRevoked { get; set; }

        public UserDB User { get; set; } = null!;
    }
}
