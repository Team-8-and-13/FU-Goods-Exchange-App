using Microsoft.EntityFrameworkCore;
using FUExchange.Contract.Services.Interface;
using FUExchange.Repositories.Context;
using FUExchange.Services;
using FUExchange.Services.Service;

namespace FUExchangeBE.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddDatabase(configuration);
            //services.AddIdentity();
            services.AddInfrastructure(configuration);
            services.AddServices();
        }
        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("MyCnn"),
                     options => options.MigrationsAssembly("FUExchange.Contract.Repositories"));
            });
        }

        //public static void AddIdentity(this IServiceCollection services)
        //{
        //    services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        //    {
        //    })
        //     .AddEntityFrameworkStores<DatabaseContext>()
        //     .AddDefaultTokenProviders();
        //}
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProImagesService, ProductImageService>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<ICommentService, CommentService>();
        }
    }
}
