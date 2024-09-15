using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Repositories.UOW;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.Services.Service;

namespace FUExchange.Services
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories();
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
        }
    }
}
