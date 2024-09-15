using Microsoft.AspNetCore.Identity;
using FUExchange.ModelViews.AuthModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginModelView model);
        Task<IdentityResult> RegisterAsync(RegisterModelView model);
    }
}
