using AutoMapper;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.ModelViews.ProductModelViews;

namespace FUExchange.Services.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            //Product
            CreateMap<SelectProductModelView, Product>().ReverseMap();
        }
    }
}
