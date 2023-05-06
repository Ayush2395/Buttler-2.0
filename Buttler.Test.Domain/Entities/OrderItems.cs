using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buttler.Test.Domain.Entities
{
    public class OrderItems
    {
        [Key]
        public int OrderItemsId { get; set; }

        [Column("Order Master Id")]
        public int? OrderMasterId { get; set; }
        public OrderMaster OrderMaster { get; set; }

        [Column("Food Id")]
        public int? FoodsId { get; set; }
        public Foods? Foods { get; set; }

        public int? Quantity { get; set; }

    }
}
