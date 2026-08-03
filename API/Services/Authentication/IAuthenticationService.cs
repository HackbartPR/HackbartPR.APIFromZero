using FluentResults;
using Microsoft.AspNetCore.Identity.Data;

namespace API.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result> Login(LoginRequest request, CancellationToken cancellationToken);

        Task<Result> RefreshToken(string token, CancellationToken cancellationToken);

        Task Logout(string token, CancellationToken cancellationToken);
    }
}
