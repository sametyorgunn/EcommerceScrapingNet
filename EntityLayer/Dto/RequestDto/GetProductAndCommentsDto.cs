using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.RequestDto
{
    public class GetProductAndCommentsDto
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        //public string CategoryName { get; set; }
        public int? ProductId { get; set; }
    }
}
