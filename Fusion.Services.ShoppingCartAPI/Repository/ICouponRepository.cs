using Fusion.Services.ShoppingCartAPI.Models.DTO;

namespace Fusion.Services.ShoppingCartAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCoupon(string couponName);
    }
}
