using Buttler.Test.Application.DTO;
using Buttler.Test.Domain.Data;

namespace Buttler.Test.Application.Repositories
{
    public class PlaceOrderRepo : IPlaceOrderRepo
    {
        private readonly ButtlerContext _context;

        public PlaceOrderRepo(ButtlerContext context)
        {
            _context = context;
        }

        public ResultDto<string> PlaceOrderByCustomer(PlaceOrderDto order, string userId)
        {
            throw new NotImplementedException();
        }

        //public ResultDto<string> PlaceOrderByCustomer(PlaceOrderDto order, string userId)
        //{
        //    var tableId = _context.Tables.FirstOrDefault(r => r.CustomerId == order.customerId);
        //    if (tableId!=null)
        //    {
        //        _context.OrderMasters.Add(new()
        //        {

        //        });
        //    }

        //}
    }

    public interface IPlaceOrderRepo
    {
        ResultDto<string> PlaceOrderByCustomer(PlaceOrderDto order, string userId);
    }
}
