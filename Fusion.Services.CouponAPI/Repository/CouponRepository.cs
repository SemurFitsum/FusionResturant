using AutoMapper;
using Fusion.Services.CouponAPI.DbContexts;
using Fusion.Services.CouponAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Fusion.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<CouponDTO> GetCouponByCode(string couponCode)
        {
            var couponFromDb = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponCode == couponCode);
            return _mapper.Map<CouponDTO>(couponFromDb);
        }
    }
}
