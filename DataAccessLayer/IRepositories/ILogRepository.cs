﻿using EntityLayer.Dto.RequestDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepositories
{
	public interface ILogRepository
	{
		Task AddLog(Log log);
	}
}
