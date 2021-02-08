
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMTDigital.Model;
using MMTDigital.Services;
using MMTDigital.ViewModel;


namespace MMTDigital.Controllers
{
    public class CustomerOrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public CustomerOrderController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        // POST: /<controller>/
        [HttpPost]
        [Route("api/CustomerOrder/GetOrder")]
        public async Task<ActionResult> GetOrder([FromBody] UserView userView)
        {
            var result = await _orderService.GetRecentOrder(userView);

            switch (result.customer.CustomerStatus)
            {
                case CustomerStatus.InvalidUser:
                    return NotFound("Invalid User");
                case CustomerStatus.UnauthorizedAccess:
                    return Unauthorized();
                case CustomerStatus.UnknownError:
                    return BadRequest("request is incorrect");
                default:
                    var orderSummary = _mapper.Map<CustomerRecentOrderView>(result);
                    return Ok(orderSummary);
            }
        }
    }
}
