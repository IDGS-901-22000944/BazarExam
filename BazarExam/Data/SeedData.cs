using System.Text.Json;
using System.Text.Json.Serialization;
using BazarExam.Models;

namespace BazarExam.Data
{
    public static class SeedData
    {
        public static async Task LoadProducts(ApplicationDbContext context)
        {
            if (context.Products.Any()) return;

            // Leer el archivo JSON
            var json = await File.ReadAllTextAsync("products.json");

            // Deserializar correctamente
            var wrapper = JsonSerializer.Deserialize<ProductsWrapper>(json);

            if (wrapper?.Products != null && wrapper.Products.Any())
            {
                context.Products.AddRange(wrapper.Products);
                await context.SaveChangesAsync();
            }
        }
    }

    // Clase auxiliar para que lea la estructura del archivo JSON
    public class ProductsWrapper
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }
    }
}
