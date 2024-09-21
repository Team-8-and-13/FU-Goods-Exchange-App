using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;

namespace FUExchange.Contract.Services.Interface
{
    public interface ICommentService
    {
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment> CreateReplyCommentAsync(string commentId, Comment comment);
        Task<Comment> GetCommentByIdAsync(string id);
        Task<IEnumerable<Comment>> GetAllCommentsFromProductAsync(string id);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(string id);
        Task<BasePaginatedList<Comment>> GetCommentPaginatedAsync(int pageIndex, int pageSize);
    }
}
