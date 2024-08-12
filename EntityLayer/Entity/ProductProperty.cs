using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class ProductProperty
    {
        public int Id { get; set; }
        public string PropertyTitle { get; set; }
        public string PropertyText { get; set; }
        public int ProductId { get; set; }
    }
}
