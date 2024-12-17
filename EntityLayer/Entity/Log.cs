using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
	public class Log
	{
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
		public DateTime CreatedDate { get; set; }
    }
}
