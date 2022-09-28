using Fusion.Services.CouponAPI.Models.DTO;

namespace Fusion.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCode(string couponCode);
    }
}
