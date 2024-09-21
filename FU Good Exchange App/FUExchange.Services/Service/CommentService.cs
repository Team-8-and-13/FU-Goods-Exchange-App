using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using Microsoft.EntityFrameworkCore;

namespace FUExchange.Services.Service
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsFromProductAsync(string id)
        {
            return await _unitOfWork.GetRepository<Comment>().Entities.Where(c => c.ProductId == id).ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id);
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            await _unitOfWork.GetRepository<Comment>().InsertAsync(comment);
            await _unitOfWork.SaveAsync();
            return comment;
        }

        public async Task<Comment> CreateReplyCommentAsync(string commentId, Comment comment)
        {
            await _unitOfWork.GetCommentRepository().CreateReplyCommentForComment(commentId, comment);
            return comment;
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            comment.LastUpdatedTime = DateTime.Now;
            _unitOfWork.GetRepository<Comment>().UpdateAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _unitOfWork.GetRepository<Comment>().DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<Comment>> GetCommentPaginatedAsync(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Comment>().Entities;
            return await _unitOfWork.GetRepository<Comment>().GetPagging(query, pageIndex, pageSize);
        }
    }
}
