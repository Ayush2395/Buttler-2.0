using Buttler.Test.Application.DTO;
using Buttler.Test.Domain.Data;

namespace Buttler.Test.Application.Repositories
{
    public class TablesRepo : ITablesRepo
    {
        private readonly ButtlerContext _context;

        public TablesRepo(ButtlerContext context)
        {
            _context = context;
        }

        public ResultDto<TablesDto> BookTableForCustomer(TablesDto bookTable, int customerId)
        {
            var customer = _context.Customers.Any(rec => rec.CustomerId == customerId);
            if (customer && bookTable != null)
            {
                _context.Tables.Add(new()
                {
                    TableNumber = bookTable.TableNumber,
                    CustomerId = customerId,
                });
                _context.SaveChanges();
                return new ResultDto<TablesDto>(true, "Table booked");
            }
            else
            {
                return new ResultDto<TablesDto>(false, "Please enter table number");
            }
        }
    }

    public interface ITablesRepo
    {
        ResultDto<TablesDto> BookTableForCustomer(TablesDto bookTable, int customerId);
    }
}
