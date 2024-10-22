using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.ResponseDto
{
	public class ScrapingResponseDto
	{
        public string Status { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
    }
}
