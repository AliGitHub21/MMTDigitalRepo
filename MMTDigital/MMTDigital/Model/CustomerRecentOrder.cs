using System;
using MMTDigital.ViewModel;

namespace MMTDigital.Model
{
    public class CustomerRecentOrder
    {
		public Customer customer { get; set; }
		public OrderView order { get; set; }
	}
}
