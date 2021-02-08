using System;
using System.Collections.Generic;

namespace MMTDigital.ViewModel
{
    public class OrderView
    {
        public OrderView()
        {
            this.orderItems = new List<ProductView>();
        }
        public int orderNumber { get; set; }
		public string orderDate { get; set; }
		public string deliveryAddress { get; set; }
		public List<ProductView> orderItems { get; set; }
		public string deliveryExpected { get; set; }
    }
}
