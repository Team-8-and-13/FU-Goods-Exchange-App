﻿using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface ICategoryService
    {
        Task<BasePaginatedList<Category>> GetAllCategories(int pageIndex, int pageSize);
        Task<CategoriesModelView?> GetCategoryById(string id);
        Task CreateCategory(CreateCategoryModelViews createCategoryModel);
        Task UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel);
        Task<Category> DeleteCategory(string id);
        Task<BasePaginatedList<Product>> GetAllProductsbyIdCategory(string categoryId, int pageIndex, int pageSize);
    }
}
