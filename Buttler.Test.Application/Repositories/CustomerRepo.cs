using Buttler.Test.Application.DTO;
using Buttler.Test.Domain.Data;
using Newtonsoft.Json;

namespace Buttler.Test.Application.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ButtlerContext _context;

        public CustomerRepo(ButtlerContext context)
        {
            _context = context;
        }

        public List<CustomerDto> GetAllCustomer()
        {
            return _context.Customers.Select(r => new CustomerDto
            {
                Id = r.CustomerId,
                CustomerName = r.CustomerName,
                CustomerGender = r.Gender,
                PhoneNumber = r.CustomerPhoneNumber
            }).ToList();
        }

        public string TakeUserDetail(CustomerDto customer)
        {
            _context.Customers.Add(new()
            {
                CustomerName = customer.CustomerName,
                CustomerPhoneNumber = customer.PhoneNumber,
                Gender = customer.CustomerGender
            });
            var currentCustomer = _context.Customers.Select(r => r.CustomerId).LastOrDefault();
            _context.SaveChanges();
            return JsonConvert.SerializeObject(new { data = currentCustomer, message = "Customer added." });
        }
    }

    public interface ICustomerRepo
    {
        string TakeUserDetail(CustomerDto customer);
        List<CustomerDto> GetAllCustomer();
    }
}
