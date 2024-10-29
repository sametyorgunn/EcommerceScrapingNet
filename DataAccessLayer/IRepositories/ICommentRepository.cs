using DataAccessLayer.IRepositories.IGeneric;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepositories
{
	public interface ICommentRepository:IGenericRepository<Comment>
	{
		public Task<Comment> InsertComment(Comment comment);
	}
}
