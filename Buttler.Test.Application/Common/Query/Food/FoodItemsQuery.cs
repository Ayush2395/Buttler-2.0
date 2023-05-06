using Buttler.Test.Application.DTO;
using Buttler.Test.Application.Repositories;
using MediatR;

namespace Buttler.Test.Application.Common.Query.Food
{
    public class FoodItemsQuery : IRequest<ResultDto<List<FoodsDto>>>
    {
        public class Handler : IRequestHandler<FoodItemsQuery, ResultDto<List<FoodsDto>>>
        {
            private readonly IFoodRepo _foodRepo;

            public Handler(IFoodRepo foodRepo)
            {
                _foodRepo = foodRepo;
            }

            public Task<ResultDto<List<FoodsDto>>> Handle(FoodItemsQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_foodRepo.GetAllFoodItems());
            }
        }
    }
}
