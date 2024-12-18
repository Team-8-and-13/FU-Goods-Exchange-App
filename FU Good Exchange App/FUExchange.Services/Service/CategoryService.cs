﻿using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BasePaginatedList<CategoriesModelView>> GetAllCategories(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Category>().Entities.Where(p => !p.DeletedTime.HasValue);
            IQueryable<Category> CateQuery = _unitOfWork.GetRepository<Category>()
                .Entities
                .Where(i => i.DeletedTime == null);
            var paginatedList = await _unitOfWork.GetRepository<Category>().GetPagging(CateQuery, pageIndex, pageSize);
            var mappedList = paginatedList.Items.Select(c => new CategoriesModelView
            {
                CategoryId = c.Id.ToString(),
                Name = c.Name
            }).ToList();
            return new BasePaginatedList<CategoriesModelView>(mappedList, paginatedList.TotalItems, paginatedList.CurrentPage, paginatedList.PageSize);
        }

        public async Task<CategoriesModelView?> GetCategoryById(string id)
        {
            var category =  _unitOfWork.GetRepository<Category>().Entities.Where(i => i.Id == id && !i.DeletedTime.HasValue).FirstOrDefault() 
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Danh mục đã bị xóa hoặc không tồn tại!"); ;
            var cate = new CategoriesModelView
            {
                CategoryId = category.Id,
                Name = category.Name
            };
            return cate;
        }

        public async Task CreateCategory(CreateCategoryModelViews createCategoryModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            if (createCategoryModel.Name.IsNullOrEmpty())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Tên không được để trống."); ;
            }
            var category = new Category
            {
                Name = createCategoryModel.Name,
                CreatedBy = userID.ToString(),
                CreatedTime = DateTime.Now
            };
            await _unitOfWork.GetRepository<Category>().InsertAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            var existingCategory = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);

            if (updateCategoryModel.Name.IsNullOrEmpty())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Tên không được rỗng."); 
            }
            if (existingCategory == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Danh mục không tồn tại!");
            }
            else if (existingCategory.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Danh mục đã bị xóa!");
            }
            existingCategory.Name = updateCategoryModel.Name;
            existingCategory.LastUpdatedBy = userID.ToString();
            existingCategory.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategory(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);

            if (category == null || category.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Danh mục đã bị xóa hoặc không tồn tại!");
            }

            category.DeletedBy = userID.ToString();
            category.DeletedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<Category>> GetCategoryPaginated(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Category>().Entities;
            return await _unitOfWork.GetRepository<Category>().GetPagging(query, pageIndex, pageSize);
        }

        public async Task<BasePaginatedList<ProductByCategoryModelViews>> GetAllProductsbyIdCategory(string categoryId, int pageIndex, int pageSize)
        {
            var findIDCate = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
            if (findIDCate == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Danh mục không tồn tại");
            }
            else if (findIDCate.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Danh mục đã bị xóa");
            }
            var query = _unitOfWork.GetRepository<Product>().Entities
                                  .Where(p => !p.DeletedTime.HasValue && p.CategoryId == categoryId);

            if (!await query.AnyAsync())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không có sản phẩm cho danh mục này");
            }

            var paginatedList = await _unitOfWork.GetRepository<Product>().GetPagging(query, pageIndex, pageSize);

            // Ánh xạ từ Product sang SelectProductModelView
            var mappedList = paginatedList.Items.Select(p => new ProductByCategoryModelViews
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryName = findIDCate.Name // Lấy tên danh mục từ findIDCate
            }).ToList();
            return new BasePaginatedList<ProductByCategoryModelViews>(mappedList, paginatedList.TotalItems, paginatedList.CurrentPage, paginatedList.PageSize);
        }

    }
}