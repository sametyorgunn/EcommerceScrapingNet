using BusinessLayer.IServices.IGeneric;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
	public interface ICommentService:IGenericService<CommentDto>
	{
		public Task<CommentDto> InsertComment(CommentDto comment);
		public Task<Dictionary<string, List<CommentDto>>> GetCommentByPrediction(CommentDto comment);
	}
}
