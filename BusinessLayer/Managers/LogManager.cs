using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
	public class LogManager : ILogService
	{
		private readonly ILogRepository _logRepository;
		private readonly IMapper _mapper;
		public LogManager(ILogRepository logRepository, IMapper mapper)
		{
			_logRepository = logRepository;
			_mapper = mapper;
		}

		public async Task AddLog(LogDto log)
		{
			var payload = _mapper.Map<Log>(log);
			await _logRepository.AddLog(payload);
		}
	}
}
