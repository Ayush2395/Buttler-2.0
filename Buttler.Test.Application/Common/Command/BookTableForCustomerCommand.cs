using Buttler.Test.Application.DTO;
using Buttler.Test.Application.Repositories;
using MediatR;

namespace Buttler.Test.Application.Common.Command
{
    public class BookTableForCustomerCommand : IRequest<ResultDto<TablesDto>>
    {
        public int CustomerId { get; set; }
        public TablesDto BookTable { get; set; }
        public class Handler : IRequestHandler<BookTableForCustomerCommand, ResultDto<TablesDto>>
        {

            private readonly ITablesRepo _tablesRepo;

            public Handler(ITablesRepo tablesRepo)
            {
                _tablesRepo = tablesRepo;
            }

            public Task<ResultDto<TablesDto>> Handle(BookTableForCustomerCommand request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_tablesRepo.BookTableForCustomer(request.BookTable, request.CustomerId));
            }
        }
    }
}
