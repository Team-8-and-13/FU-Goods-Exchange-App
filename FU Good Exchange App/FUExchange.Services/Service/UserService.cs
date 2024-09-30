using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Utils;
using FUExchange.ModelViews.UserModelViews;
using FUExchange.Repositories.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FUExchange.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IList<UserResponseModel>> GetAll()
        {
            var users = await _userManager.Users
                .Include(u => u.UserInfo)
                .ToListAsync();

            var userResponseList = new List<UserResponseModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userResponseList.Add(new UserResponseModel
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Username = user.UserName,
                    FullName = user.UserInfo?.FullName,
                    Roles = roles
                });
            }

            return userResponseList;
        }

        public async Task<UserResponseModel?> GetById(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.UserInfo)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserResponseModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Username = user.UserName,
                FullName = user.UserInfo?.FullName,
                Roles = roles
            };
        }

        public async Task<bool> CreateUser(CreateUserModel model, string? adminId)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                CreatedBy = adminId,
                CreatedTime = CoreHelper.SystemTimeNow,
                UserInfo = new UserInfo
                {
                    FullName = model.FullName
                }
            };

            // Hash the password
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = model.Role });
                }

                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                return roleResult.Succeeded;
            }
            return false;
        }

        public async Task<bool> UpdateUser(string userId, UpdateUserModel model, string? adminId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.LastUpdatedBy = adminId;
            user.LastUpdatedTime = CoreHelper.SystemTimeNow;

            // If password needs to be updated, hash the new password
            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            }

            // Role management: check if role update is needed
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(model.Role))
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeRolesResult.Succeeded)
                {
                    return false;
                }

                // Add new role
                var addRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!addRoleResult.Succeeded)
                {
                    return false;
                }
            }
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }


        public async Task<bool> DeleteUser(string userId, string adminId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.DeletedBy = adminId;
            user.DeletedTime = CoreHelper.SystemTimeNow;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<UserResponseModel?> GetById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            return new UserResponseModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Username = user.UserName,
                FullName = user.UserInfo?.FullName
            };
        }
    }

}
