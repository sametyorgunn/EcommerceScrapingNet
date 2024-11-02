using AutoMapper;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.MappingProfiles
{
	public class CommentProfile:Profile
	{
		public CommentProfile()
		{
			CreateMap<Comment, CommentDto>().ReverseMap();
			CreateMap<CommentDto, CommentEmotionDto>().ReverseMap();
			CreateMap<Comment, CommentEmotionDto>().ReverseMap();
			CreateMap<CommentDto, SentimentPredictionDto>().ReverseMap();
			CreateMap<Comment, SentimentPredictionDto>().ReverseMap();
		}
	}
}
