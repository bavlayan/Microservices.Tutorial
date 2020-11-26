using Basket.API.DataLayer.Interfaces;
using Basket.API.Entities;
using Basket.API.Repository.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _basketContext;

        public BasketRepository(IBasketContext basketContext)
        {
            _basketContext = basketContext;
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            return await _basketContext.Redis.KeyDeleteAsync(userName);
        }

        public async Task<BasketCart> GetBasketCart(string userName)
        {
            RedisValue basket = await _basketContext.Redis.StringGetAsync(userName);

            if (basket.IsNullOrEmpty)
                return null;
            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basketCart)
        {
            var updated = await _basketContext.Redis.StringSetAsync(basketCart.UserName, JsonConvert.SerializeObject(basketCart));
            if (updated)
                return await GetBasketCart(basketCart.UserName);
            return null;
        }
    }
}
