using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class TrendyolCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public List<TrendyolCategory> SubCategories { get; set; }
    }
    public class TrendyolCategoryResponse
    {
        public List<TrendyolCategory> TrendyolCategories { get; set; }
    }
}

