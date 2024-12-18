﻿using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.PaggingItems;
using FUExchange.Core;
using FUExchange.ModelViews.ProductModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.Contract.Services.Interface
{
    public interface IProductService
    {
        Task<BasePaginatedList<Product>> GetAllProductsFromModerator(int pageIndex, int pageSize);
        Task<PaginatedList<SelectProductModelView>> GetAllProductsFromUser(int pageIndex, int pageSize);
        Task<SelectProductModelView?> GetProductByIdAsync(string id);
        Task CreateProduct(CreateProductModelView createProductModelView);
        Task UpdateProduct(string productId, UpdateProductModelView updateProductModelView);
        Task DeleteProduct(string id);
        Task ApproveProduct(string id);
        Task SoldProduct(string id);
        Task RateProduct(string id, int star);
        Task<SelectProductModelView?> GetProductByCommentId(string id);
    }
}
