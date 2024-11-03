using BusinessLayer.IServices.IGeneric;
using EntityLayer.Dto;
using EntityLayer.Dto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
	public interface IUserService:IGenericService<UserDto>
	{
		Task<LoginResponseDto> SignIn(UserDto userDto);
	}
}
