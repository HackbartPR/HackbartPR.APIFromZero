using API.Constants;
using API.CrossCutting.BaseControllers;
using API.CrossCutting.BaseResponses;
using API.Exceptions;
using API.Services.Authentication;
using Asp.Versioning;
using Domain.Constants;
using Domain.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.V1.Authentication
{
    /// <summary>
    /// Controller conterá todos os endpoints relacionados a autenticação, registro, passwords ...
    /// </summary>
    /// <param name="logger"></param>
    [ApiVersion("1.0")]
    public class AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService auth) : BaseAPIController(logger)
    {
        private readonly IAuthenticationService _auth = auth ?? throw new ArgumentNullException(nameof(auth), "Serviço não inicializado.");

        /// <summary>
        /// Endpoint utilizado apenas para testes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Teste()
        {
            return ApplicationResponse(new BaseResponse<string>
            {
                Success = true,
                Message = "Teste realizado na Versão 1.0",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Endpoint utilizado apenas simular um erro não tratado.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Exception")]
        public IActionResult TesteException()
        {
            string? strSample = null;

            return ApplicationResponse(new BaseResponse<int>
            {
                Data = strSample.Length,
                Success = true,
                Message = "Teste realizado na Versão 1.0",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Endpoint utilizado apenas teste de idempotência..
        /// </summary>
        /// <returns></returns>
        [HttpPost("Idempotency")]
        public IActionResult TesteIdempotency()
        {
            return ApplicationResponse(new BaseResponse<string>
            {
                Success = true,
                Message = "Requisição recebida com sucesso",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Endpoint utilizado apenas para teste de autenticação e autorização
        /// </summary>
        /// <returns></returns>
        [HttpGet("login-admin")]
        [Authorize(Roles = RoleConstants.Admin)]
        public IActionResult TesteLoginAdmin()
        {
            return ApplicationResponse(new BaseResponse<string>
            {
                Success = true,
                Message = "Teste realizado com sucesso",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Autentica o usuário e emite um token JWT armazenado em um cookie HttpOnly.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [EndpointName("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login, CancellationToken cancellationToken = default)
        {
            Result result = await _auth.Login(login, cancellationToken);

            if (result.IsFailed)
            {
                return ApplicationResponse(new BaseResponse<string>
                {
                    Success = false,
                    Errors = result.GetErrors(),
                    StatusCode = result.GetStatusCode(),
                    Message = "Não foi possível realizar o login"
                });
            }

            return ApplicationResponse(new BaseResponse<string>
            {
                Success = true,
                Message = "Login realizado com sucesso!",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Realiza o logout do usuário logado removendo o Access e Refresh Token Cookie.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Logout")]
        [EndpointName("Logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
        {
            string? refreshToken = Request.Cookies[AuthenticationConstants.RefreshTokenCookie];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return ApplicationResponse(new BaseResponse<string>
                {
                    Success = true,
                    Message = "Logout realizado com sucesso!",
                    StatusCode = HttpStatusCode.OK
                });
            }

            await _auth.Logout(refreshToken, cancellationToken);

            return ApplicationResponse(new BaseResponse<string>
            {
                Success = true,
                Message = "Logout realizado com sucesso!",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Realiza a renovação da sessão utilizando o refresh token e gera
        /// um novo access token e um novo refresh token.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("refresh")]
        [EndpointName("refresh")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken = default)
        {
            string? refreshToken = Request.Cookies[AuthenticationConstants.RefreshTokenCookie];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                Result tempResult = Result.Fail(APIError.UnauthorizedRefreshToken);
                return ApplicationResponse(new BaseResponse<string>
                {
                    Success = false,
                    Errors = tempResult.GetErrors(),
                    StatusCode = tempResult.GetStatusCode(),
                    Message = "Não foi possível autenticar o refresh token"
                });
            }

            Result result = await _auth.RefreshToken(refreshToken, cancellationToken);

            if (result.IsFailed)
            {
                return ApplicationResponse(new BaseResponse<string>
                {
                    Success = false,
                    Errors = result.GetErrors(),
                    StatusCode = result.GetStatusCode(),
                    Message = "Não foi possível autenticar o refresh token"
                });
            }

            return ApplicationResponse(new BaseResponse<string>
            {
                Success = true,
                Message = "Token renovado com sucesso!",
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
