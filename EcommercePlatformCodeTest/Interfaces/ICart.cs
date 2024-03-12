using EcommercePlatformCodeTest.Dtos;
using EcommercePlatformCodeTest.Models;

namespace EcommercePlatformCodeTest.Interfaces
{
    public interface ICart
    {
        Task<(int, List<CartItem>)> CartItems(int startIndex, int showCount, string searchString);
        Task AddToCart(Product product, int userId, int quantity);
        Task RemoveCart(int id);
        Task UpdateCartItemQuantity(int id, int quantity);
        Task<Product?> GetProductByIdAsync(int productId);

        Task<CBPaymentReqVM> GetQrString_FromCBBank(int userId, decimal amount);
        Task<CBPayCheckResponse> CheckTransaction_FromCBBank(string transactionRefNo, int userId);
        Task<List<UserTransaction>> GetUserTransaction(int userId);
    }
}
