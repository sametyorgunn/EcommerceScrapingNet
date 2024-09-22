using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.RequestDto.Product
{
	public class GetProductByFilterDto
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string ProductBrand { get; set; }
		public string ProductName { get; set; }
		public string ProductRating { get; set; }
		public string ProductPrice { get; set; }
		public string ProductImage { get; set; }
		public int PlatformId { get; set; }
		public int CategoryId { get; set; }
	}
}
