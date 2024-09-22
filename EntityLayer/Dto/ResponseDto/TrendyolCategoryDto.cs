using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.ResponseDto
{
    public class TrendyolCategoryDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? parentId { get; set; }
        public List<TrendyolCategoryDto> subCategories { get; set; }
    }
    public class TrendyolCategoryResponseDto
    {
        public List<TrendyolCategoryDto> categories { get; set; }
    }
}
