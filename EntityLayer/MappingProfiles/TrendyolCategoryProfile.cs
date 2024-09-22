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
    public class TrendyolCategoryProfile:Profile
    {
        public TrendyolCategoryProfile()
        {
            CreateMap<TrendyolCategory, TrendyolCategoryDto>().ReverseMap();
            //CreateMap<TrendyolCategory, TrendyolCategoryResponseDto>().ReverseMap();
        }
    }
}
