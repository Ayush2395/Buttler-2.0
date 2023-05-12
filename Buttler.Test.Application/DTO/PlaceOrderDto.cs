using Buttler.Test.Domain.Entities;

namespace Buttler.Test.Application.DTO
{
    public class PlaceOrderDto
    {
        public int customerId { get; set; }
        List<FoodsDto> Foods { get; set; }

    }
}
