using System;
using System.Net.Http;
using System.Threading.Tasks;
using MMTDigital.Model;
using MMTDigital.ViewModel;

namespace MMTDigital.Services
{
    public interface IOrderService
    {
        public Task<CustomerRecentOrder> GetRecentOrder(UserView userDetails);
        public Task<Customer> CheckCustomerDetails(UserView userDetails);
        HttpResponseMessage GetCustomerDetailsApi(UserView userDetails);
        public OrderView GetOrderDetails(UserView userDetails, string deliveryAddress);
    }
}
