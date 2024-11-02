using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
	public class CommentManager : ICommentService
	{
		private readonly ICommentRepository _commentRepository;
		private readonly IMapper _mapper;
		public CommentManager(ICommentRepository commentRepository, IMapper mapper)
		{
			_commentRepository = commentRepository;
			_mapper = mapper;
		}

		public async Task<Dictionary<string, List<CommentDto>>> GetCommentByPrediction(CommentDto comment)
		{
			var payload = _mapper.Map<Comment>(comment);
			var comments = _commentRepository.GetCommentByPrediction(payload);
				var groupedComments = comments.Result
			   .GroupBy(c => c.Prediction.ToString()) 
			   .ToDictionary(
				   group => group.Key,
				   group => _mapper.Map<List<CommentDto>>(group.ToList())
			   );

			return groupedComments;
		}

		public Task<List<CommentDto>> GetListAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<CommentDto> InsertComment(CommentDto comment)
		{
			var payload = _mapper.Map<Comment>(comment);
			var result = await _commentRepository.InsertComment(payload);
			var response = _mapper.Map<CommentDto>(result);
			return response;
		}

		public Task TAddAsync(CommentDto t)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> TAddRangeAsync(List<CommentDto> t)
		{
			var payload = _mapper.Map<List<Comment>>(t);
			var result = await _commentRepository.InsertManyAsync(payload);
			//var response = _mapper.Map<List<CommentDto>>(result);
			return result;
		}

		public Task TDeleteAsync(CommentDto t)
		{
			throw new NotImplementedException();
		}

		public Task<CommentDto> TGetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task TUpdateAsync(CommentDto t)
		{
			throw new NotImplementedException();
		}

		public Task<bool> TUpdateRangeAsync(List<CommentDto> t)
		{
			throw new NotImplementedException();
		}
	}
}
