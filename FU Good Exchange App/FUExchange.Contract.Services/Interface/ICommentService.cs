using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.CommentModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface ICommentService
    {
        Task CreateCommentAsync(CreateCommentModelViews viewModel);
        Task CreateReplyCommentAsync(string repCommentId, CreateReplyCommentModelView viewModel);
        Task<Comment?> GetCommentByIdAsync(string id);
        Task<IEnumerable<Comment>> GetAllCommentsFromProductAsync(string productid);
        Task UpdateCommentAsync(string id, CreateCommentModelViews viewModel);
        Task DeleteCommentAsync(string id);
        Task<BasePaginatedList<Comment>> GetCommentPaginatedAsync(int pageIndex, int pageSize);
    }
}
