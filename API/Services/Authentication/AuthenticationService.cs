using API.Constants;
using API.Services.JWT;
using API.Services.JWT.DTOs;
using Domain.Exceptions.Base;
using FluentResults;
using Infrastructure.Databases.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System.Net;

namespace API.Services.Authentication
{
    /// <summary>
    /// Este serviço foi baseado na documentação do IDentity, portanto nota-se que utilizamos muitas classes derivadas da lib IDentity.
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="jwtService"></param>
    /// <param name="httpContextAccessor"></param>
    public class AuthenticationService(UserManager<UserDB> userManager, SignInManager<UserDB> signInManager, IJWTService jwtService, IHttpContextAccessor httpContextAccessor) : IAuthenticationService
    {
        private readonly UserManager<UserDB> _userManager = userManager ?? throw new ArgumentNullException(nameof(UserManager<UserDB>), "Serviço não inicializado.");
        private readonly SignInManager<UserDB> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(SignInManager<UserDB>), "Serviço não inicializado.");
        private readonly IJWTService _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService), "Serviço não inicializado.");
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor), "Serviço não inicializado.");

        /// <summary>
        /// Autentica o usuário e emite um token JWT armazenado em um cookie HttpOnly.
        /// Estamos utilizando a estrutura base do IDentity, mas com algumas modificações para podermos trabalhar com JWT | Cookies | Resposta padronizada.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            bool isPersistent = false;

            UserDB? user = await _userManager.FindByEmailAsync(request.Email);

            SignInResult result = user == null
                ? await _signInManager.CheckPasswordSignInAsync(new UserDB(), request.Password, lockoutOnFailure: true)
                : await _signInManager.CheckPasswordSignInAsync(user!, request.Password, lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(request.TwoFactorCode))
                {
                    result = await signInManager.TwoFactorAuthenticatorSignInAsync(request.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(request.TwoFactorRecoveryCode))
                {
                    result = await signInManager.TwoFactorRecoveryCodeSignInAsync(request.TwoFactorRecoveryCode);
                }
            }

            if (!result.Succeeded)
                return Result.Fail(new BaseError(result.ToString(), HttpStatusCode.Unauthorized));

            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtService.GetTokenExpiration())
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(_jwtService.GetRefreshTokenExpiration())
            };

            TokenResponse tokens = await _jwtService.GenerateTokensAsync(user!, cancellationToken);

            var response = _httpContextAccessor.HttpContext!.Response;
            response.Cookies.Append(AuthenticationConstants.TokenCookie, tokens.AccessToken, accessTokenOptions);
            response.Cookies.Append(AuthenticationConstants.RefreshTokenCookie, tokens.RefreshToken, refreshTokenOptions);

            return Result.Ok();
        }

        /// <summary>
        /// Realiza o logout do usuário logado removendo o Access e Refresh Token Cookie.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Logout(string token, CancellationToken cancellationToken)
        {
            await _jwtService.RevokeRefreshToken(token, cancellationToken);

            var response = _httpContextAccessor.HttpContext!.Response;
            response.Cookies.Delete(AuthenticationConstants.TokenCookie);
            response.Cookies.Delete(AuthenticationConstants.RefreshTokenCookie);
        }

        /// <summary>
        /// Realiza a renovação da sessão utilizando o refresh token e gera
        /// um novo access token e um novo refresh token.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result> RefreshToken(string token, CancellationToken cancellationToken)
        {
            Result<TokenResponse> result = await _jwtService.RefreshToken(token, cancellationToken);

            if (result.IsFailed)
                return Result.Fail(result.Errors);

            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtService.GetTokenExpiration())
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(_jwtService.GetRefreshTokenExpiration())
            };

            var response = _httpContextAccessor.HttpContext!.Response;
            response.Cookies.Append(AuthenticationConstants.TokenCookie, result.Value.AccessToken, accessTokenOptions);
            response.Cookies.Append(AuthenticationConstants.RefreshTokenCookie, result.Value.RefreshToken, refreshTokenOptions);

            return Result.Ok();
        }
    }
}
