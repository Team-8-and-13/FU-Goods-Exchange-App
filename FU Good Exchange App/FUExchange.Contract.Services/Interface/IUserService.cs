using FUExchange.ModelViews.UserModelViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUExchange.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<IList<UserResponseModel>> GetAll();
        Task<UserResponseModel?> GetById(Guid userId);
        Task<bool> CreateUser(CreateUserModel model, string? adminId);
        Task<bool> UpdateUser(string id, UpdateUserModel model, string? adminId);
        Task<bool> DeleteUser(string id, string? adminId);
        Task<UserResponseModel?> GetById(string id);
    }
}
