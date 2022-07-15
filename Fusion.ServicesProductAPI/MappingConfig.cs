using AutoMapper;
using Fusion.ServicesProductAPI.Models;
using Fusion.ServicesProductAPI.Models.DTO;

namespace Fusion.ServicesProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>();
                config.CreateMap<Product, ProductDTO>();
            });

            return mappingConfig;
        }
    }
}
