using AutoMapper;
using Fusion.Services.CouponAPI.Models;
using Fusion.Services.CouponAPI.Models.DTO;

namespace Fusion.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDTO>().ReverseMap();
                //config.CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();
                //config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
                //config.CreateMap<Cart, CartDTO>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
