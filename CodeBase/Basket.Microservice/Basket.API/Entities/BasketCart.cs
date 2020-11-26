using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class BasketCart
    {
        public string UserName { get; set; }

        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        public decimal TotalPrice 
        { 
            get
            {
                return CalculatePriceBasketItems();
            }
            
        }

        public BasketCart()
        {

        }

        public BasketCart(string userName)
        {
            UserName = userName;
        }


        private decimal CalculatePriceBasketItems()
        {
            decimal totalPrice = 0;
            foreach (BasketItem basketItem in BasketItems)
            {
                totalPrice = totalPrice + (basketItem.Price * basketItem.Quantity);
            }
            return totalPrice;
        }
    }
}
