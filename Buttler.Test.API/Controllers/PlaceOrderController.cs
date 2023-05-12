using Buttler.Test.Application.Common.Command;
using Buttler.Test.Application.DTO;
using Buttler.Test.Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buttler.Test.API.Controllers
{
    [Authorize(Roles = "staff,admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceOrderController : BaseController
    {
        public readonly ICustomerRepo _repo;

        public PlaceOrderController(ICustomerRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("CustomerDetails")]
        public async Task<ActionResult> CustomerDetails(CustomerDto customer)
        {
            if (customer != null)
            {
                var result = await Mediator.Send(new TakeCustomerDetailsCommand { Customer = customer });
                return Ok(result);
            }
            else
            {
                return BadRequest("User details required.");
            }
        }

        [HttpPost("BookTable")]
        public async Task<ActionResult> BookTableForCustomer(TablesDto tables, int customerId)
        {
            var result = await Mediator.Send(new BookTableForCustomerCommand { BookTable = tables, CustomerId = customerId });
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetCustomerDetails")]
        public ActionResult GetCustomers()
        {
            var result = _repo.GetAllCustomer();
            return result != null ? Ok(result) : BadRequest();
        }
    }
}
