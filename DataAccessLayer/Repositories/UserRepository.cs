using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository:GenericRepository<User>, IUserRepository
	{
		private readonly AppDbContext _appDbContext;

		public UserRepository(AppDbContext appDbContext) : base(appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task<LoginResponseDto> SignIn(User user)
		{
			var UserControl = _appDbContext.users.Where(x=>x.UserName == user.UserName &&
			x.Password == user.Password &&
			x.Status == true)
			 .FirstOrDefault();
			if(UserControl != null)
			{
				return new LoginResponseDto
				{
					Id = UserControl.Id,
					LoginStatus = 1,
					UserName = user.UserName
				};
			}
			else
			{
				return new LoginResponseDto() { LoginStatus = 0 };
			}
		}
	}
}
