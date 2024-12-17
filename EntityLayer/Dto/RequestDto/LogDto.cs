﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto.RequestDto
{
	public class LogDto
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public string Source { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
