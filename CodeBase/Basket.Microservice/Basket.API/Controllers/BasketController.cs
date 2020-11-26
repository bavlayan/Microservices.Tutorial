using Basket.API.Entities;
using Basket.API.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasketCart(string userName)
        {
            BasketCart basketCart = await _basketRepository.GetBasketCart(userName);
            return Ok(basketCart ?? new BasketCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasketCart([FromBody] BasketCart basketCart)
        {
            BasketCart _basketCart = await _basketRepository.UpdateBasket(basketCart);
            return Ok(_basketCart);
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasketCart(string userName)
        {
            bool isDeleted = await _basketRepository.DeleteBasket(userName);
            return Ok(isDeleted);
        }
    }
}
