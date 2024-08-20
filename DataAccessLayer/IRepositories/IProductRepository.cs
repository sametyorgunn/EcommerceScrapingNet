using DataAccessLayer.IRepositories.IGeneric;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<List<Product>> GetListAllByPlatformIdAsync(int platformId);
		Task<Product> GetProductByProductId(GetProductByProductId request);
		Task<List<Product>> GetLastFiveProducts();


	}
}
