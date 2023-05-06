using Buttler.Test.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace Buttler.Test.Domain.Entities
{
    public class OrderMaster
    {
        [Key]
        public int Id { get; set; }

        [Column("Customer Id")]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Column("Staff Id", TypeName = "nvarchar")]
        [StringLength(450)]
        public string? StaffId { get; set; }


        [Column("Table Id")]
        public int? TablesId { get; set; }
        public Tables Tables { get; set; }

        [Column("Order Status")]
        public Enums.Enums.Orderstatus? OrderStatus { get; set; } = Enums.Enums.Orderstatus.pending;

        [Column("Bill")]
        public decimal? TotalBill { get; set; }

        [Column("Date of Order")]
        public DateTime? DateOfOrder { get; set; }
    }
}
