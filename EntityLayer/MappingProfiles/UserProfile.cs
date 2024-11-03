using AutoMapper;
using EntityLayer.Dto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.MappingProfiles
{
	public class UserProfile:Profile
	{
	   public UserProfile() 
		{
			CreateMap<User, UserDto>().ReverseMap();
			CreateMap<UserDto, LoginResponseDto>().ReverseMap();
			CreateMap<User, LoginResponseDto>().ReverseMap();
		}
	}
}
