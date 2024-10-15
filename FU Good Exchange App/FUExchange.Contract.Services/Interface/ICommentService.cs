using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.CommentModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface ICommentService
    {
        Task CreateComment(CreateCommentModelViews viewModel);
        Task CreateReplyComment(string repCommentId, CreateReplyCommentModelView viewModel);
        Task<CommentModelView?> GetCommentById(string id);
        Task<ICollection<FormatFBComment>> GetAllCommentsFromProduct(string productid, int pageIndex, int pageSize);
        Task UpdateComment(string id, CreateCommentModelViews viewModel);
        Task DeleteComment(string id);
    }
}
