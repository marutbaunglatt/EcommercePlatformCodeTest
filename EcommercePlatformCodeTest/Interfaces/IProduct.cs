using EcommercePlatformCodeTest.Models;

namespace EcommercePlatformCodeTest.Infterfaces
{
    public interface IProduct
    {
        Task<(int, List<Product>)> GetAllProductsAsync(int startIndex, int showCount, string searchString, string categor);
        Task AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
