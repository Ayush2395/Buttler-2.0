using System.Text.Json.Serialization;

namespace Buttler.Test.Application.DTO
{
    public class TablesDto
    {
        public int TableNumber { get; set; }
        [JsonIgnore]
        public int CustomerId { get; set; }
    }
}
