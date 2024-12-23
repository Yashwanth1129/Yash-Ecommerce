using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Data.SeedData
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("C:\\Project\\Ecommerce\\API\\WebApplication1\\Data\\SeedData\\products.json");

                var products=JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products == null) return;

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
