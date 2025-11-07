using System.Text.Json;
using BazarExam.Models;

namespace BazarExam.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Products.Any()) return;

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "products.json");
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine("❌ Archivo products.json no encontrado.");
                return;
            }

            var json = File.ReadAllText(jsonPath);
            var jsonDoc = JsonDocument.Parse(json);
            var products = new List<Product>();

            foreach (var element in jsonDoc.RootElement.GetProperty("products").EnumerateArray())
            {
                try
                {
                    products.Add(new Product
                    {
                        Title = element.TryGetProperty("title", out var title) ? title.GetString() ?? "" : "",
                        Description = element.TryGetProperty("description", out var description) ? description.GetString() ?? "" : "",
                        Category = element.TryGetProperty("category", out var category) ? category.GetString() ?? "" : "",
                        Price = element.TryGetProperty("price", out var price) ? price.GetDecimal() : 0,
                        DiscountPercentage = element.TryGetProperty("discountPercentage", out var discount) ? discount.GetDouble() : 0,
                        Rating = element.TryGetProperty("rating", out var rating) ? rating.GetDouble() : 0,
                        Stock = element.TryGetProperty("stock", out var stock) ? stock.GetInt32() : 0,
                        Brand = element.TryGetProperty("brand", out var brand) ? brand.GetString() ?? "" : "",
                        Thumbnail = element.TryGetProperty("thumbnail", out var thumb) ? thumb.GetString() ?? "" : ""
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Error leyendo un producto: {ex.Message}");
                }
            }

            _context.Products.AddRange(products);
            _context.SaveChanges();
            Console.WriteLine($"✅ {products.Count} productos cargados correctamente.");
        }
    }
}
