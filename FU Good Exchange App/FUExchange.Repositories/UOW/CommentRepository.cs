using Castle.Core.Smtp;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.IUOW;
using FUExchange.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.Repositories.UOW
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public readonly DatabaseContext _context;
        public CommentRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentFromProduct(string id)
        {
            return await _context.Comments.Where(c => c.ProductId == id).ToListAsync();
        }

        public async Task<Comment> CreateReplyCommentForComment(string commentId, Comment comment)
        {
            var CommentIsReply = await GetByIdAsync(commentId);
            comment.RepCmtId = commentId;
            comment.ProductId = CommentIsReply.ProductId;
            await InsertAsync(comment);
            await SaveAsync();
            return comment;
        }
    }
}
