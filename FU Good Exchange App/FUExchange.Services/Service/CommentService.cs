using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.CommentModelViews;
using FUExchange.Repositories.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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

            //Them notification cho seller
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            Product product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(viewModel.ProductId) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, ErrorMessages.NOT_FOUND);
            if(userId != product.SellerId)
            {
                Notification notification = new Notification
                {
                    UserId = product.SellerId, // Thông báo cho người dùng nào
                    ProductId = product.Id, // Thông báo ở sản phẩm nào
                    Content = User.Identity?.Name + " đã bình luận vào sản phẩm " 
                              + product.Name + " của bạn." //Thông báo nội dung gì
                };
                await _unitOfWork.GetRepository<Notification>().InsertAsync(notification);
            }
            //Kết thúc notification

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

            //Thêm notification cho phần phản hồi
            Comment repcmt = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(repCommentId) ??
                throw new ArgumentException("Couldn't find comment to reply");
            
            if(userId != repcmt.UserId) //Không thông báo khi tự phản hồi chính mình
            {
                Notification notification = new Notification
                {
                    UserId = repcmt.UserId,
                    ProductId = repcmt.ProductId,
                    Content = User.Identity?.Name + " đã phản hồi bình luận của bạn."
                };
                await _unitOfWork.GetRepository<Notification>().InsertAsync(notification);

                //Thông báo cho Seller các phản hồi của các user khác trong sản phẩm
                Product product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(repcmt.ProductId) ??
                    throw new ArgumentException("Không tìm thấy sản phẩm");
                var replyUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == repcmt.UserId) ??
                    throw new ArgumentException("Không tìm thấy người dùng");

                if (userId != product.SellerId) // Shop phản hồi ko cần lưu thông báo
                {
                    Notification notificationforSeller = new Notification
                    {
                        UserId = product.SellerId,
                        ProductId = repcmt.ProductId,
                        Content = User.Identity?.Name + " đã phản hồi bình luận của " + replyUser.UserName
                    };
                    await _unitOfWork.GetRepository<Notification>().InsertAsync(notificationforSeller);
                }
            } 
            //Kết thúc notification

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
