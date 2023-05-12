using Buttler.Test.Application.DTO;
using Buttler.Test.Application.Repositories;
using MediatR;

namespace Buttler.Test.Application.Common.Command
{
    public class TakeCustomerDetailsCommand : IRequest<string>
    {
        public CustomerDto Customer { get; set; }
        public class Handler : IRequestHandler<TakeCustomerDetailsCommand, string>
        {
            private readonly ICustomerRepo _customerRepo;

            public Handler(ICustomerRepo customerRepo)
            {
                _customerRepo = customerRepo;
            }

            public Task<string> Handle(TakeCustomerDetailsCommand request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_customerRepo.TakeUserDetail(request.Customer));
            }
        }
    }
}
