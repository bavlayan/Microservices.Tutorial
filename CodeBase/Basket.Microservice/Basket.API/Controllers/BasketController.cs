using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repository.Interfaces;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
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

        private readonly IMapper _mapper;

        private readonly EventBusRabbitMQProducer _eventBusRabbitMQProducer;

        public BasketController(IBasketRepository basketRepository, IMapper mapper, EventBusRabbitMQProducer eventBusRabbitMQProducer)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _eventBusRabbitMQProducer = eventBusRabbitMQProducer;
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

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckOut([FromBody]BasketCheckOut basketCheckOut)
        {
            BasketCart basket = await _basketRepository.GetBasketCart(basketCheckOut.UserName);
            if (basket == null)
                return BadRequest();

            bool isRemoved = await _basketRepository.DeleteBasket(basketCheckOut.UserName);
            if (!isRemoved)
                return BadRequest();

            BasketCheckoutEvent eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckOut);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                _eventBusRabbitMQProducer.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception)
            {
                throw;
            }

            return Accepted();

        }
    }
}
