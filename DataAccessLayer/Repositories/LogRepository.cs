using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public class LogRepository : ILogRepository
	{
		private readonly AppDbContext _appDbContext;

		public LogRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		public async Task AddLog(Log log)
		{
			await _appDbContext.Logs.AddAsync(log);
			await _appDbContext.SaveChangesAsync();
		}
	}
}
