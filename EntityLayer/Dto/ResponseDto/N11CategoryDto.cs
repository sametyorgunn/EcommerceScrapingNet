using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.ResponseDto
{
	public class N11CategoryDto
	{
		public string CategoryName { get; set; }
		public List<N11SubCategoryDto> N11SubCategories { get; set; }
	}
	public class N11SubCategoryDto
	{
		public string CategoryName { get; set; }
	}
}
