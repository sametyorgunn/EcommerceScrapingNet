using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.ResponseDto
{
	public class CategoryMarketPlaceDto
	{
		public string CategoryName { get; set; }
        public int PlatformID { get; set; }
        public List<SubCategoryMarketPlaceDto> SubCategories { get; set; }
	}
	public class SubCategoryMarketPlaceDto
	{
        public int PlatformID { get; set; }

        public string CategoryName { get; set; }
	}
}
