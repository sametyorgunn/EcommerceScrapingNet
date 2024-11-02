using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public class CommentRepository :GenericRepository<Comment>, ICommentRepository
	{
		private readonly AppDbContext _appDbContext;

		public CommentRepository(AppDbContext appDbContext) : base(appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task<List<Comment>> GetCommentByPrediction(Comment comment)
		{
			var comments = await _appDbContext.comments.Where(x=>x.ProductId == comment.ProductId).ToListAsync();
			return comments;
		}

		public async Task<Comment> InsertComment(Comment comment)
		{
			_appDbContext.comments.Add(comment);
			_appDbContext.SaveChanges();
			return comment;
		}
	}
}
