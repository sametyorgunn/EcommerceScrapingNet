using AutoMapper;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.MappingProfiles
{
	public class LogProfile:Profile
	{
		public LogProfile()
		{
			CreateMap<Log, LogDto>().ReverseMap();
		}
	}
}
