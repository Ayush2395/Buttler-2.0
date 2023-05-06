using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buttler.Test.Domain.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Column("Customer Name", TypeName = "varchar")]
        [MaxLength(30)]
        public string? CustomerName { get; set; }

        [Column("Gender", TypeName = "varchar")]
        [MaxLength(10)]
        public string? Gender { get; set; }

        [Column("Customer Phone Number", TypeName = "varchar")]
        [MaxLength(10)]
        public string? CustomerPhoneNumber { get; set; }
    }
}
