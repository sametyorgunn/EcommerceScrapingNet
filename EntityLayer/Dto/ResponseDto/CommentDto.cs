using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.ResponseDto
{
    public class CommentDto
    {
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string CommentText { get; set; }
		public string? ProductLink { get; set; }
	}
}
