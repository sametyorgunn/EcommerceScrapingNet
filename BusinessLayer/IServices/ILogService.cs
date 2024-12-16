using EntityLayer.Dto.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
	public interface ILogService
	{
		Task AddLog(LogDto log);
	}
}
