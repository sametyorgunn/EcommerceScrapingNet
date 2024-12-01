using AutoMapper;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryResponseDto>().ReverseMap();

            CreateMap<N11Category, N11CategoryDto>().ReverseMap();
            CreateMap<N11SubCategory, N11SubCategoryDto>().ReverseMap();
		}
    }
}
