using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.ModelViews.AuthModelViews;
using FUExchange.Contract.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FUExchange.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;

namespace FUExchange.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> LoginAsync(LoginModelView model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    return string.Empty; // Invalid credentials
                }

                // Check if the user has been blocked (reported)
                bool reported = _unitOfWork.GetRepository<Report>().Entities.Any(u => u.UserId == user.Id && u.Status);
                if (reported)
                {
                    return user.UserName + " has been blocked !!!";
                }

                var roles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.UtcNow.AddHours(10),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public async Task<IdentityResult> RegisterAsync(RegisterModelView model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                UserInfo = new UserInfo { FullName = model.FullName }
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign default role "UserPolicy" after successful registration
                if (await _roleManager.RoleExistsAsync(ApplicationRole.UserPolicy))
                {
                    await _userManager.AddToRoleAsync(user, ApplicationRole.UserPolicy);
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Default role 'UserPolicy' does not exist."
                    });
                }
            }
            return result;
        }
    }
}
