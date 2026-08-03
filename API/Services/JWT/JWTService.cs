using API.Exceptions;
using API.Services.JWT.DTOs;
using API.Services.JWT.Settings;
using FluentResults;
using Infrastructure.Databases.Contexts;
using Infrastructure.Databases.Entities.Token;
using Infrastructure.Databases.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Services.JWT
{
    /// <summary>
    /// Define operações para geração, renovação e obtenção das configurações de expiração de tokens de autenticação JWT.
    /// Esse service não possui obrigação de saber como o token será utilizado: via Header/Cookie/...
    /// </summary>
    /// <param name="options"></param>
    /// <param name="userManager"></param>
    /// <param name="context"></param>
    public class JWTService(IOptions<JWTOptions> options, UserManager<UserDB> userManager, EFContext context) : IJWTService
    {
        private readonly JWTOptions _options = options.Value ?? throw new ArgumentNullException(nameof(IOptions<JWTOptions>), "Serviço não inicializado.");
        private readonly UserManager<UserDB> _userManager = userManager ?? throw new ArgumentNullException(nameof(UserManager<UserDB>), "Serviço não inicializado.");
        private readonly EFContext _context = context ?? throw new ArgumentNullException(nameof(context), "Serviço não inicializado.");

        /// <summary>
        /// Recupera o Tempo de Expiração do Token de Acesso configurado nas variáveis de ambiente.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetTokenExpiration()
        {
            if (string.IsNullOrWhiteSpace(_options.ExpirationInMinutes))
                throw new Exception("'Expiration Time' não foi configurado");

            return int.Parse(_options.ExpirationInMinutes);
        }

        /// <summary>
        /// Gera os tokens de Acesso e Renovação
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TokenResponse> GenerateTokensAsync(UserDB user, CancellationToken cancellationToken)
        {
            string jti = Guid.NewGuid().ToString();

            return new TokenResponse
            {
                AccessToken = await GenerateAccessTokenAsync(user, jti),
                RefreshToken = await GenerateRefreshTokenAsync(user, jti, cancellationToken)
            };
        }

        /// <summary>
        /// Recupera o Tempo de Expiração do Token de Renovação configurado nas variáveis de ambiente.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetRefreshTokenExpiration()
        {
            if (string.IsNullOrWhiteSpace(_options.RefreshExpirationInDays))
                throw new Exception("'Expiration Time' não foi configurado");

            return int.Parse(_options.RefreshExpirationInDays);
        }

        /// <summary>
        /// Gera o token de acesso.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="jti"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<string> GenerateAccessTokenAsync(UserDB user, string jti)
        {
            if (string.IsNullOrWhiteSpace(_options.SecretKey) || string.IsNullOrWhiteSpace(_options.Issuer) || string.IsNullOrWhiteSpace(_options.Audience))
                throw new Exception("Uma ou mais configurações obrigatórias do JWT não foram definidas");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_options.ExpirationInMinutes ??= "0"));

            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            foreach (string role in userRoles)
                userClaims.Add(new Claim(ClaimTypes.Role, role));

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: userClaims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Gera o token de renovação
        /// </summary>
        /// <param name="user"></param>
        /// <param name="jti"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<string> GenerateRefreshTokenAsync(UserDB user, string jti, CancellationToken cancellationToken)
        {
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(64);

            string token = Convert.ToBase64String(tokenBytes);

            string tokenHash = Convert.ToBase64String(SHA256.HashData(tokenBytes));

            var expires = DateTime.UtcNow.AddDays(double.Parse(_options.RefreshExpirationInDays ??= "1"));

            _context.RefreshTokens.Add(new RefreshTokenDB
            {
                Id = Guid.CreateVersion7(),
                UserId = user.Id,
                Token = tokenHash,
                Jti = jti,
                ExpiresAt = expires,
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
            });

            await _context.SaveChangesAsync(cancellationToken);

            return token;
        }

        /// <summary>
        /// Valida e Gera novos tokens durante o processo de renovação do Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<TokenResponse>> RefreshToken(string token, CancellationToken cancellationToken)
        {
            byte[] tokenByte = Convert.FromBase64String(token);

            string tokenHashed = Convert.ToBase64String(SHA256.HashData(tokenByte));

            RefreshTokenDB? tokenDB = await _context.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token.Equals(tokenHashed), cancellationToken);

            if (tokenDB == null || tokenDB.IsRevoked || tokenDB.ExpiresAt < DateTime.UtcNow)
                return Result.Fail(APIError.UnauthorizedRefreshToken);

            tokenDB.IsRevoked = true;

            TokenResponse newTokens = await GenerateTokensAsync(tokenDB.User, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return newTokens;
        }

        /// <summary>
        /// Revoga o token de renovação
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RevokeRefreshToken(string token, CancellationToken cancellationToken)
        {
            byte[] tokenByte = Convert.FromBase64String(token);

            string tokenHashed = Convert.ToBase64String(SHA256.HashData(tokenByte));

            RefreshTokenDB? tokenDB = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token.Equals(tokenHashed), cancellationToken);

            if (tokenDB == null || tokenDB.IsRevoked || tokenDB.ExpiresAt < DateTime.UtcNow)
                return;

            tokenDB.IsRevoked = true;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
