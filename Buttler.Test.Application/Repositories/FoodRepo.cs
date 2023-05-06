using Buttler.Test.Application.DTO;
using Buttler.Test.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buttler.Test.Application.Repositories
{
    public class FoodRepo : IFoodRepo
    {
        private readonly ButtlerContext _context;

        public FoodRepo(ButtlerContext context)
        {
            _context = context;
        }

        public ResultDto<List<FoodsDto>> GetAllFoodItems()
        {
            var foodItems = _context.Foods.Select(rec => new FoodsDto
            {
                Id = rec.FoodId,
                FoodImg = rec.FoodImage,
                Title = rec.Title,
                Description = rec.Description,
                Price = rec.Price,
            }).ToList();

            return new ResultDto<List<FoodsDto>>(true, foodItems);
        }
    }

    public interface IFoodRepo
    {
        ResultDto<List<FoodsDto>> GetAllFoodItems();
    }
}
