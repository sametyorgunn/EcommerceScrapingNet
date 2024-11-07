using BusinessLayer.IServices.IGeneric;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
	public interface IAmazonService
	{
		public Task<ScrapingResponseDto> GetProductAndCommentsAsync(GetProductAndCommentsDto request);
	}
}
