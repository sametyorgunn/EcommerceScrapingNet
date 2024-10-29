using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Entity;
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

		public async Task<Comment> InsertComment(Comment comment)
		{
			_appDbContext.comments.Add(comment);
			_appDbContext.SaveChanges();
			return comment;
		}
	}
}
