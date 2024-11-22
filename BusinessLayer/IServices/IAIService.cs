using EntityLayer.Dto.RequestDto.Product;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface IAIService
    {
        public Task<bool> isTrueProduct(isTrueProductDto dto);
    }
}
