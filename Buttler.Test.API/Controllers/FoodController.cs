using Buttler.Test.Application.Common.Query.Food;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Buttler.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : BaseController
    {
        [HttpGet("FoodItems")]
        public async Task<ActionResult> GetFoodItems()
        {
            var result = await Mediator.Send(new FoodItemsQuery());
            return result != null ? Ok(result) : BadRequest();
        }
    }
}
