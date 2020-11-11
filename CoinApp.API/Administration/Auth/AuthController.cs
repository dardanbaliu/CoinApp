using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoinApp.API.Administration.Auth.Commands;
using CoinApp.API.Administration.Auth.Queries;
using CoinApp.API.Extensions;
using CoinApp.API.Helper;
using CoinApp.API.Utils;
using CoinApp.Infrastructure.JwtAuthentication.Abstractions;
using CoinApp.SharedModel.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoinApp.API.Administration.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Token _token;
        private readonly IMediator _mediator;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthController(IOptions<Token> token, IMediator mediator, IJwtTokenGenerator jwtTokenGenerator)
        {
            _token = token.Value;
            _mediator = mediator;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        //refresh expired token
        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody]RefreshTokenDto refreshToken)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(refreshToken.Token);

                if (principal == null)
                    return Ok(new
                    {
                        RefreshToken = refreshToken.RefreshToken,
                        Token = refreshToken.Token,
                        IsRefreshTokenChanged = false
                    });

                var username = principal.Identity.Name;
                var savedRefreshToken = _mediator.Send(new GetRefreshToken(username)).Result;
                if (savedRefreshToken != refreshToken.RefreshToken)
                    return Unauthorized();

                var newJwtToken = _jwtTokenGenerator.GenerateAccessTokenWithClaimsPrincipal(username, principal.Claims);
                var newRefreshToken = GenerateTokenHelper.GenerateRefreshToken();
                var response = _mediator.Send(new SaveRefreshToken(username, newRefreshToken)).Result;

                return Ok(new
                {
                    RefreshToken = newRefreshToken,
                    Token = newJwtToken.AccessToken,
                    IsRefreshTokenChanged = true
                });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized();
            }

        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_token.SigningKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            long unixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

            if (jwtSecurityToken.Payload.Exp > unixSeconds && (jwtSecurityToken.Payload.Exp - unixSeconds) > 1800)
                return null;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        
    }
}