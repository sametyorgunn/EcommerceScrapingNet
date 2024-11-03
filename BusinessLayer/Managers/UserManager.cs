using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
using EntityLayer.Dto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
	public class UserManager : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		public UserManager(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public Task<List<UserDto>> GetListAsync()
		{
			throw new NotImplementedException();
		}

		public Task<LoginResponseDto> SignIn(UserDto userDto)
		{
			var payload = _mapper.Map<User>(userDto);
			var login = _userRepository.SignIn(payload);
			return login;
		}

		public Task TAddAsync(UserDto t)
		{
			throw new NotImplementedException();
		}

		public Task<bool> TAddRangeAsync(List<UserDto> t)
		{
			throw new NotImplementedException();
		}

		public Task TDeleteAsync(UserDto t)
		{
			throw new NotImplementedException();
		}

		public Task<UserDto> TGetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task TUpdateAsync(UserDto t)
		{
			throw new NotImplementedException();
		}

		public Task<bool> TUpdateRangeAsync(List<UserDto> t)
		{
			throw new NotImplementedException();
		}
	}
}
