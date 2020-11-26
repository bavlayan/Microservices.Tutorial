using Basket.API.DataLayer.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.DataLayer
{
    public class BasketContext : IBasketContext
    {
        public IDatabase Redis { get; }

        private readonly ConnectionMultiplexer _redisConnection;

        public BasketContext(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;

            Redis = _redisConnection.GetDatabase();
        }
    }
}
