using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
	public class CategoryMarketPlace
	{
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int PlatformID { get; set; }
        public ICollection<SubCategoryMarketPlace> SubCategories { get; set; }
	}
	public class SubCategoryMarketPlace
	{
		public int Id { get; set; }
		public string CategoryName { get; set; }
		public int CategoryId { get; set; }
        public int PlatformID { get; set; }

        public CategoryMarketPlace Category { get; set; }
	}
}
