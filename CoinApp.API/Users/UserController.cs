using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinApp.API.Administration.Auth.Commands;
using CoinApp.API.Helper;
using CoinApp.API.Users.Commands;
using CoinApp.Infrastructure.JwtAuthentication.Abstractions;
using CoinApp.SharedModel.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinApp.API.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public UserController(IMediator mediator, IJwtTokenGenerator jwtTokenGenerator)
        {
            this._mediator = mediator;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        //register new user
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Register(NewUserDto request)
        {
            try
            {
                //var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var response = await _mediator.Send(new Register(request.FirstName, request.LastName, request.Username, request.Email, request.Password));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var user = await _mediator.Send(new Login(request.Username, request.Password));
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        var userInfo = new UserDto
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Username = user.Username,
                        };

                        var userClaim = AddMyClaims(userInfo);
                        var newClaim = new List<Claim>();
                        newClaim.AddRange(userClaim);
                        var accessToken = _jwtTokenGenerator.GenerateAccessTokenWithClaimsPrincipal(request.Username, newClaim);
                        var newRefreshToken = GenerateTokenHelper.GenerateRefreshToken();
                        var response = _mediator.Send(new SaveRefreshToken(userInfo.Username, newRefreshToken)).Result;

                        userInfo.AccessToken = accessToken.AccessToken;
                        userInfo.RefreshToken = newRefreshToken;
                        return Ok(userInfo);
                    }
                    else
                    {
                        throw new Exception("User is not active!");
                    }
                }
                throw new Exception("Username or Password is incorrect!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static IEnumerable<Claim> AddMyClaims(UserDto user)
        {
            var myClaims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Username", user.Username)
            };
            return myClaims;
        }
    }
}
