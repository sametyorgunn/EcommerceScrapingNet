﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.RequestDto.Category
{
	public class AddCategoryDto
	{
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
	}
}
