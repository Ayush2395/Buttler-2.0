using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buttler.Test.Domain.Entities
{
    public class Tables
    {
        [Key]
        public int TablesId { get; set; }

        [Column("Table Number")]
        public int? TableNumber { get; set; }

        [Column("Customer Id")]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
