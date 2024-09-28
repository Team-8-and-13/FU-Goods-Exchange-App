using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.CommentModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsFromProductAsync(string productid)
        {
            return await _unitOfWork.GetRepository<Comment>().Entities.Where(c => c.ProductId == productid).ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id) ??
                  throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy comment");
        }

        public async Task CreateCommentAsync(CreateCommentModelViews viewModel)
        {
            if (viewModel == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, ErrorMessages.ERROR);
            }

            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;

            var comment = new Comment
            {
                ProductId = viewModel.ProductId,
                CommentText = viewModel.CommentText,
                UserId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value),
                CreatedBy = User.Identity?.Name,
            };
            comment.RepCmtId = comment.Id;

            await _unitOfWork.GetRepository<Comment>().InsertAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateReplyCommentAsync(string repCommentId, CreateReplyCommentModelView viewModel)
        {
            if (viewModel == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, ErrorMessages.ERROR);
            }

            if (_unitOfWork.GetCommentRepository().GetByIdAsync(repCommentId) == null)
            {
                throw new ArgumentException("Couldn't find comment to reply");
            }

            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var comment = new Comment
            {
                CommentText = viewModel.CommentText,
                UserId = userId,
                CreatedBy = User.Identity?.Name,
            };

            await _unitOfWork.GetCommentRepository().CreateReplyCommentForComment(repCommentId, comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCommentAsync(string id,CreateCommentModelViews viewModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var comment = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id);

            if (comment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Comment");
            }

            if (comment.UserId != userId) // Chỉ user tạo comment mới được cập nhật comment
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Unauthorized !!!");
            }

            comment.CommentText = viewModel.CommentText;
            comment.LastUpdatedBy = User.Identity?.Name;
            comment.LastUpdatedTime = DateTime.Now;

            await _unitOfWork.GetRepository<Comment>().UpdateAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCommentAsync(string id)
        {
            var comment = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id);
            if (comment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Comment");
            }

            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var userName = User.Identity?.Name;
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            if (comment.UserId != userId)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Unauthorized !!!");
            }
            else
            {
                comment.DeletedBy = userName;
                await _unitOfWork.GetRepository<Comment>().UpdateAsync(comment);
                await _unitOfWork.SaveAsync();
            }
        }
        public async Task<BasePaginatedList<Comment>> GetCommentPaginatedAsync(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Comment>().Entities;
            return await _unitOfWork.GetRepository<Comment>().GetPagging(query, pageIndex, pageSize);
        }
    }
}
