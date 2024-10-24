using BusinessLayer.IServices.IGeneric;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.RequestDto.Product;
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
        //Task<ProductDto> GetProductByProductId(GetProductByProductId request);
		Task<List<ProductDto>> GetProductsByPlatformId(GetProductsByPlatformId request);
		Task<List<ProductDto>> GetLastFiveProducts();
        Task<List<ProductDto>> GetProductsByCategoryId(GetProductByFilterDto request);
        Task<List<ProductDto>> GetLastTwelveProduct();
        Task<ProductDto> GetProductWithCommentAndProperties(GetProductByFilterDto request);
        Task<ProductDto> CreateProduct(ProductDto dto);

	}
}
