﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ProductBrand { get; set; }
        public string ProductName { get; set; }
        public string ProductRating { get; set; }
        public string ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int PlatformId { get; set; }
		public bool Status { get; set; }
		public string ProductLink { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
		public List<ProductProperty>? ProductProperty { get; set; }
        public List<Comment> Comment { get; set; }
    }
}
