using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buttler.Test.Domain.Entities
{
    public class Foods
    {
        [Key]
        public int FoodId { get; set; }

        [Column("Food Image", TypeName = "nvarchar")]
        [MaxLength(128)]
        public string? FoodImg { get; set; }
        public string? Title { get; set; }
        [Column("Description", TypeName = "nvarchar")]
        [MaxLength(256)]
        public string? Description { get; set; }

        public decimal? Price { get; set; }
    }
}
