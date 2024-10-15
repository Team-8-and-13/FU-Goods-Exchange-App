using AutoMapper;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.ModelViews.BanModelViews;
using FUExchange.ModelViews.CommentModelViews;
using FUExchange.ModelViews.NotificationModelViews;
using FUExchange.ModelViews.ProductModelViews;

namespace FUExchange.Services.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            //Product
            CreateMap<SelectProductModelView, Product>().ReverseMap();

            //Notification
            CreateMap<NotificationDisplay, Notification>().ReverseMap();

            //Ban
            CreateMap<BanModelView, Ban>().ReverseMap();

            //Comment
            CreateMap<CommentModelView, Comment>().ReverseMap();
        }
    }
}
