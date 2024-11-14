using AutoMapper;
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
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ICollection<FormatFBComment>> GetAllCommentsFromProduct(string productid, int pageIndex, int pageSize)
        {
            Product? product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productid) ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "không tìm thấy sản phẩm");
            var comment = _unitOfWork.GetRepository<Comment>().Entities.Where(c => c.ProductId == productid && c.Id == c.RepCmtId && !c.DeletedTime.HasValue).OrderByDescending(c => c.CreatedTime);
            var replyComment = _unitOfWork.GetRepository<Comment>().Entities.Where(c => c.ProductId == productid && c.Id != c.RepCmtId && !c.DeletedTime.HasValue).OrderBy(c => c.CreatedTime);
            ICollection<FormatFBComment> list = new List<FormatFBComment>();
            foreach(Comment c in comment)
            {
                FormatFBComment fBComment = new FormatFBComment {
                    comment = _mapper.Map<CommentModelView>(c),
                    ReplyList = _mapper.Map<ICollection<CommentModelView>>(replyComment.Where(r => r.CommentArea == c.CommentArea))
                };
                list.Add(fBComment);
            }
            var perpage = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return perpage ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Chưa có bình luận nào cho sản phẩm này");
        }

        public async Task<CommentModelView?> GetCommentById(string id)
        {
            var comment = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id);
            if (comment == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Comment không tồn tại!");
            }
            else if (comment.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Comment đã bị xóa!");
            }

            var commentView = new CommentModelView
            {
                UserId = comment.UserId,
                Id = comment.Id,
                RepCmtId = comment.RepCmtId == null ? comment.Id : comment.RepCmtId,
                CommentText = comment.CommentText
            };

            return commentView ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy comment"); ;
        }

        public async Task CreateComment(CreateCommentModelViews viewModel)
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
            comment.CommentArea = comment.Id;

            //Them notification cho seller
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            Product product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(viewModel.ProductId) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, ErrorMessages.NOT_FOUND);
            if(userId != product.SellerId) // Không thông báo khi tạo bình luận vào bài đăng của chính mình
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

        public async Task CreateReplyComment(string repCommentId, CreateReplyCommentModelView viewModel)
        {
            //Lấy ra các thông tin cần thiết để tạo phần thông báo cho user, seller
            Comment repcmt = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(repCommentId) ??
                throw new ArgumentException("Không tìm thấy bình luận để phản hồi");
            Product product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(repcmt.ProductId) ??
                throw new ArgumentException("Không tìm thấy sản phẩm");
            var replyUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == repcmt.UserId) ??
                   throw new ArgumentException("Không tìm thấy người dùng");
            
            if (viewModel == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, ErrorMessages.ERROR);
            }

            if (_unitOfWork.GetRepository<Comment>().GetByIdAsync(repCommentId) == null)
            {
                throw new ArgumentException("Không tìm thấy bình luận để phản hồi");
            }

            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            
            

            var comment = new Comment
            {
                ProductId = repcmt.ProductId,
                CommentText = viewModel.CommentText,
                UserId = userId,
                CreatedBy = User.Identity?.Name,
            };
            comment.RepCmtId = repcmt.Id;
            comment.CommentArea = repcmt.CommentArea;
        
            //Thêm notification cho phần phản hồi           
            
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
                //Không nhận thông báo dạng này khi người đc phản hồi là Seller
                if (userId != product.SellerId && repcmt.UserId != product.SellerId) // Shop phản hồi ko cần lưu thông báo, không nhận thông báo này khi người đc phản hồi là shop,
                                                                                     // đây là thông báo tương tác giữa các ng dùng khác
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

            await _unitOfWork.GetRepository<Comment>().InsertAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateComment(string id,CreateCommentModelViews viewModel)
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

        public async Task DeleteComment(string id)
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
    }
}
