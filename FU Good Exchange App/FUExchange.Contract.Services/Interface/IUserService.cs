using FUExchange.Core;
using FUExchange.ModelViews.UserModelViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUExchange.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<BasePaginatedList<UserResponseModel>> GetAll(int pageIndex, int pageSize);
        Task<UserResponseModel?> GetById(string id);
        Task<bool> CreateUser(CreateUserModel model, string? adminId);
        Task<bool> UpdateUser(string id, UpdateUserModel model, string? adminId);
        Task<bool> DeleteUser(string id, string? adminId);
    }
}
