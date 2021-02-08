using System;
namespace MMTDigital.Model
{
    public class CustomerDetails
    {
		public string customerId { get; set; }
		public string email { get; set; }
		public bool   website { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string lastLoggedIn { get; set; }
		public string houseNumber { get; set; }
		public string street { get; set; }
		public string town { get; set; }
		public string postcode { get; set; }
		public string preferredLanguage { get; set; }
	}
}
