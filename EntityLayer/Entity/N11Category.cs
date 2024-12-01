using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
	public class N11Category
	{
        public int Id { get; set; }
        public string CategoryName { get; set; }
		public ICollection<N11SubCategory> N11SubCategories { get; set; }
	}
	public class N11SubCategory
	{
		public int Id { get; set; }
		public string CategoryName { get; set; }
		public int N11CategoryId { get; set; }
		public N11Category N11Category { get; set; }
	}
}
