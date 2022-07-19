using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Listma.Test
{
    public enum OrderState
    {
        Draft,
        Processing,
        Canceled,
        Archive
    }

    public class Order
    {
        public Order() { }

        public string Number { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Product { get; set; }
        public decimal Count { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return Count * Price; } }
        public OrderState State { get; set; }
        public string ApproveState { get; set; }
        List<String> _history = new List<string>();
        public List<String> History { get { return _history; } }

        public static Order GetOrder()
        {
            return new Order()
            {
                Address = "Bond Street 8, London, England",
                Count = 1,
                Customer = "Smith, Joe",
                Number = "1/1",
                Price = 12.2M,
                Product = "CD-RW",
                State = OrderState.Draft,
                ApproveState = ""
            };
        }
    }
}
