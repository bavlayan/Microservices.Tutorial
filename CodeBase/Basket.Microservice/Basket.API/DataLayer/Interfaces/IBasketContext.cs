﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.DataLayer.Interfaces
{
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}
