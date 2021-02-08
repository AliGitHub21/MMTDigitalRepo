using System;

namespace MMTDigital.Model
{
    public class Customer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string deliveryAddress { get; set; }
        public CustomerStatus CustomerStatus { get; set; }
    }

    public enum CustomerStatus
    {
        ValidUser, InvalidUser, UnauthorizedAccess, UnknownError
    }
}
