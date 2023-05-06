using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Buttler.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
