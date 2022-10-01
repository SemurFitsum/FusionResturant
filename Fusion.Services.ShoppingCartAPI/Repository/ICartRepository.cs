using Fusion.Services.ShoppingCartAPI.Models.DTO;

namespace Fusion.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartDTO> GetCartByUserId(string userID);
        Task<CartDTO> CreateUpdateCart(CartDTO cartDTO);
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ApplyCoupon(string userID, string couponCode);
        Task<bool> RemoveCoupon(string userID);
        Task<bool> ClearCart(string userID);
    }
}
