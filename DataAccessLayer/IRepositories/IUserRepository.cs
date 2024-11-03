using DataAccessLayer.IRepositories.IGeneric;
using EntityLayer.Dto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<LoginResponseDto> SignIn(User user);

    }
}
