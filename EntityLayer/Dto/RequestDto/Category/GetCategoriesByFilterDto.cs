﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.RequestDto.Category
{
	public class GetCategoriesByFilterDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? ParentId { get; set; }
	}
}
