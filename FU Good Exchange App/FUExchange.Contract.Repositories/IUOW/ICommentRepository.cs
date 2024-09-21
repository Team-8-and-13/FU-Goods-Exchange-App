using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.Contract.Repositories.IUOW
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetAllCommentFromProduct(string id);
        Task<Comment> CreateReplyCommentForComment(string commentId, Comment comment);
    }
}
