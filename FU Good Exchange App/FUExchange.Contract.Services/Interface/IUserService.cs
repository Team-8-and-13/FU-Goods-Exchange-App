using FUExchange.ModelViews.UserModelViews;


namespace FUExchange.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<IList<UserResponseModel>> GetAll();
    }
}
