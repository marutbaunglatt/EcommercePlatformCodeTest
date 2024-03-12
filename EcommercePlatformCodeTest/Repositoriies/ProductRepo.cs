using EcommercePlatformCodeTest.Data;
using EcommercePlatformCodeTest.Infterfaces;
using EcommercePlatformCodeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatformCodeTest.Repositories
{
    public class ProductRepo : IProduct
    {
        private readonly ApplicationDbContext _context;

        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(int, List<Product>)> GetAllProductsAsync(int startIndex, int showCount, string searchString, string categor)
        {
            var res = await _context.Products.AsNoTracking()
                .Where(p => p.Stock > 0 &&
                      (!string.IsNullOrWhiteSpace(searchString) ? (p.Title.ToLower().Contains(searchString) || p.Description.ToLower().Contains(searchString)) : true) &&
                      (!string.IsNullOrWhiteSpace(categor)? p.Category == categor:true))
                      .ToListAsync();
            int totalCount = res.Count();

            res = res.Skip((startIndex - 1) * showCount)
                     .Take(showCount)
                     .ToList();
            return (totalCount, res);
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
