using API.Services.JWT.DTOs;
using FluentResults;
using Infrastructure.Databases.Entities.Users;

namespace API.Services.JWT
{
    public interface IJWTService
    {
        int GetTokenExpiration();

        int GetRefreshTokenExpiration();

        Task<TokenResponse> GenerateTokensAsync(UserDB user, CancellationToken cancellationToken);

        Task<Result<TokenResponse>> RefreshToken(string token, CancellationToken cancellationToken);

        Task RevokeRefreshToken(string token, CancellationToken cancellationToken);
    }
}
