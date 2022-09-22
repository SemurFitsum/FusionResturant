using AutoMapper;
using Fusion.Services.ShoppingCartAPI.Models;
using Fusion.Services.ShoppingCartAPI.Models.DTO;

namespace Fusion.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDTO>().ReverseMap();
                config.CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
                config.CreateMap<Cart, CartDTO>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
