using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BazarExam.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public double DiscountPercentage { get; set; }
        public double Rating { get; set; }
        public int Stock { get; set; }
        public string Brand { get; set; }
        public string Thumbnail { get; set; }

        // ✅ Esto es lo importante
        [NotMapped]
        [JsonPropertyName("images")]  // hace que lea bien el campo del JSON
        public string[] Images { get; set; } = Array.Empty<string>(); // evita que sea null
    }
}
