using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.ResponseDto
{
	public class PagedList<T>
	{
		public List<T> Items { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			Items = items;
			TotalCount = count;
			PageNumber = pageNumber;
			PageSize = pageSize;
		}

		public bool HasPreviousPage => PageNumber > 1;
		public bool HasNextPage => PageNumber < TotalPages;
	}

}
