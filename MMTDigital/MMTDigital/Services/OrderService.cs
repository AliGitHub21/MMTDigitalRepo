using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MMTDigital.Model;
using MMTDigital.ViewModel;
using System.Text.Json;
using MMTDigital.Context;

namespace MMTDigital.Services
{
    public class OrderService: IOrderService
    {
        private readonly ConnectionDB _dbConnection;
        private readonly MMTDigitalClient _MMTDigitalClient;
        private readonly Customer _customer;
        private readonly CustomerRecentOrder _customerRecentOrder;
        private readonly OrderView _orderView;
        private readonly ProductView _productView;
        private readonly IUtilityService _utilityService;

        public OrderService(ConnectionDB dbConnection, MMTDigitalClient MMTDigitalClient,  CustomerRecentOrder customerRecentOrder,
                            Customer customer, OrderView orderView, ProductView productView, IUtilityService utilityService)
        {
            _dbConnection = dbConnection;
            _MMTDigitalClient = MMTDigitalClient;
            _customer = customer;
            _customerRecentOrder = customerRecentOrder;
            _orderView = orderView;
            _productView = productView;
            _utilityService = utilityService;
        }

        /// <summary>
        /// This method get customer details and return any recent order for the customer provided with 'userDetails'
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public async Task<CustomerRecentOrder> GetRecentOrder(UserView userDetails)
        {
            _customerRecentOrder.customer = await this.CheckCustomerDetails(userDetails);

            if (_customerRecentOrder.customer.CustomerStatus == CustomerStatus.ValidUser)
            {
                _customerRecentOrder.order = this.GetOrderDetails(userDetails, _customerRecentOrder.customer.deliveryAddress);                  
            }
            return _customerRecentOrder;
        }

        /// <summary>
        /// This will check and get customer's provided details and validate them
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public async Task<Customer> CheckCustomerDetails(UserView userDetails)
        {           
            HttpResponseMessage response = GetCustomerDetailsApi(userDetails); 
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = await JsonSerializer.DeserializeAsync<CustomerDetails>(await response.Content.ReadAsStreamAsync());
                if(data.customerId == userDetails.customerId)
                {
                    _customer.firstName = data.firstName;
                    _customer.lastName = data.lastName;
                    _customer.deliveryAddress = data.houseNumber + " " + data.street + ", " + data.town + ", " + data.postcode;
                    _customer.CustomerStatus = CustomerStatus.ValidUser;
                }
                else
                {
                    _customer.CustomerStatus = CustomerStatus.InvalidUser;
                }               
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _customer.CustomerStatus = CustomerStatus.InvalidUser;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _customer.CustomerStatus = CustomerStatus.UnauthorizedAccess;             
            }
            else
            {
                _customer.CustomerStatus = CustomerStatus.UnknownError;
            }           
            return _customer;
        }

        /// <summary>
        /// Get customer details by calling Api with 'userDetails' request parameters
        /// This method uses Api-key for secure connection to an external Api Uri to check and collect user's details
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public HttpResponseMessage GetCustomerDetailsApi(UserView userDetails)
        {
            var Api_Uri = _utilityService.GetApiUri(userDetails.user);
            HttpResponseMessage response = _MMTDigitalClient.Client.GetAsync(Api_Uri).Result;
            return response;
        }

        /// <summary>
        /// This will get customer's recent order with all product details
        /// This method uses the database connection to SQL Server Database to obtain information
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="deliveryAddress"></param>
        /// <returns></returns>
        public OrderView GetOrderDetails(UserView userDetails, string deliveryAddress)
        {
            var orders = _dbConnection.Orders.Where(ord => ord.CustomerId == userDetails.customerId).ToList();

            if (orders.Count > 0)
            {
                //Find functions in IRepository
                var recentOrder = orders.OrderByDescending(item => item.OrderDate).ToList().First();
                if (recentOrder != null)
                {
                    //now populate order items with details
                    _orderView.orderNumber = recentOrder.OrderId;
                    _orderView.orderDate = recentOrder.OrderDate.ToString("dd-MMMM-yyyy");
                    _orderView.deliveryAddress = deliveryAddress;
                    _orderView.deliveryExpected = recentOrder.DeliveryExpected.ToString("dd-MMMM-yyyy");

                    // Find functions in IRepository
                    var orderItems = _dbConnection.OrderItems.Where(item => item.OrderId == recentOrder.OrderId).ToList();
                    if (orderItems != null)
                    {                    
                        foreach(var item in orderItems)
                        {
                            _productView.quantity = item.Quantity;
                            _productView.priceEach = item.Price / item.Quantity;
                            if (recentOrder.ContainsGift)
                            {
                                _productView.product = "Gift";
                            }
                            else
                            {
                                //Find functions in IRepository
                                var product = _dbConnection.Products.Where(prod => prod.ProductId == item.ProductId).First();
                                _productView.product = product.ProductName; 
                            }
                            _orderView.orderItems.Add(_productView);
                        }                       
                    }
                }            
            }
            return _orderView;
        }
    }
}
