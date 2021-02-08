using System;
using System.ComponentModel.DataAnnotations;

namespace MMTDigital.Model
{
    public class Order
    {
		[Key]
		public int OrderId { get; set; }
		public string CustomerId { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime DeliveryExpected { get; set; }
		public bool ContainsGift { get; set; }
		public string ShippingMode { get; set; }
		public string OrderSource { get; set; }

	}
}
