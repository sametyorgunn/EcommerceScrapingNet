using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.ResponseDto
{
    public class CategoryDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
		public List<CategoryDto> SubCategories { get; set; }
    }

    public class CategoryResponseDto
    {
        public List<CategoryDto> Categories { get; set; }
    }

}
