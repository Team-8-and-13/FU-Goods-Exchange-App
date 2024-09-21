using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Repositories.UOW;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.Repositories;
using FUExchange.Contract.Repositories.IUOW;


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
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProImagesRepository, ProImagesRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

        }
    }
}
