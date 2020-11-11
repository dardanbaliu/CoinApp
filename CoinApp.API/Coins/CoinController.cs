using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoinApp.API.Coins.Commands;
using CoinApp.API.Coins.Queries;
using CoinApp.Infrastructure.JwtAuthentication.Abstractions;
using CoinApp.SharedModel.Coins;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoinApp.API.Coins
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CoinController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        //list of crypto coins
        [Authorize]
        [Route("list")]
        [HttpGet]
        [ProducesResponseType(typeof(List<CoinDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListOfCoins()
        {
            try
            {
                var response = await _mediator.Send(new GetCoins());
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        //list of favorite crypto coins
        [Authorize]
        [Route("favorites")]
        [HttpGet]
        [ProducesResponseType(typeof(List<CoinDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListOfFavoriteCoins()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var response = await _mediator.Send(new GetFavoriteCoins(new Guid(userId)));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        //Save favorite crypto coins
        [Authorize]
        [Route("save")]
        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SaveFavoriteCoins(List<string> favcoinIds)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var response = await _mediator.Send(new SaveFavoriteCoins(favcoinIds, new Guid(userId)));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}