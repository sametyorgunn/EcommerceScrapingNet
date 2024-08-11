using AutoMapper;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;

namespace EntityLayer.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
