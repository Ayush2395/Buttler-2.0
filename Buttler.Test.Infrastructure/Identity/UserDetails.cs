using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buttler.Test.Infrastructure.Identity
{
    public class UserDetails
    {
        [Key]
        [Column(TypeName = "nvarchar")]
        [StringLength(450)]
        public string UId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(10)]
        public string? Gender { get; set; }

        public int? Age { get; set; }
    }
}
