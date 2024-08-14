using BusinessLayer.IServices.IGeneric;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface IProductService:IGenericService<ProductDto>
    {
        Task<ProductDto> GetProductById(GetProductById request);
        Task<List<ProductDto>> GetProductsByPlatformId(GetProductsByPlatformId request);
	}
}
