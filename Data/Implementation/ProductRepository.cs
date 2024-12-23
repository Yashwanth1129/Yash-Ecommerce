using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.Interfaces;

namespace WebApplication1.Data.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext context;

        public ProductRepository(StoreContext context)
        {
            this.context = context;
        }
        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(x => x.Brand).Distinct().ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
        {
            var query =context.Products.AsQueryable();

            if(!string.IsNullOrWhiteSpace(brand))
                    query=query.Where(x => x.Brand == brand);
            if(!string.IsNullOrWhiteSpace(type))
                query=query.Where(x =>x.Type == type);
            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.price),
                "priceDesc" => query.OrderByDescending(x => x.price),
                _ => query.OrderBy(x => x.Name),
            };

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypeAsync()
        {
            return await context.Products.Select(x => x.Type).Distinct().ToListAsync();

        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(x => x.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State=EntityState.Modified;
        }
    }
}
