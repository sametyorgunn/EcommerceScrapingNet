using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.RequestDto
{
    public class GetProductAndCommentsDto
    {
        public string TrendyolCategoryName { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
    }
}
