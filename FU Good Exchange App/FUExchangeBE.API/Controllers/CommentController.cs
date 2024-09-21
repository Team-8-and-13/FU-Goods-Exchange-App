using FUExchange.Contract.Repositories.Entity;
using FUExchange.Services.Service;
using Microsoft.AspNetCore.Mvc;
using FUExchange.ModelViews.CommentModelViews;
using FUExchange.Contract.Services.Interface;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // POST: api/Comments
        [HttpPost]
        [Route("New_Comment")]
        public async Task<IActionResult> CreateComment(CreateCommentModelViews viewModel)
        {
            if(viewModel == null)
            {
                return BadRequest();
            }
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User Name is not available in token.");
            }
            Guid userId = new Guid(User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            var comment = new Comment
            {
                ProductId = viewModel.ProductId,
                CommentText = viewModel.CommentText,
                UserId = userId,
                CreatedBy = userName
            };
            comment.RepCmtId = comment.Id;
            
            var newComment = await _commentService.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = newComment.Id }, newComment);
        }
        [HttpPost]
        [Route("Reply_Comment")]
        public async Task<IActionResult> CreateReplyomment(string repCommentId, CreateReplyCommentModelView viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User Name is not available in token.");
            }
            Guid userId = new Guid(User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            if(_commentService.GetCommentByIdAsync(repCommentId) == null)
            {
                return NotFound("Couldn't find comment to reply");
            }
            var comment = new Comment
            {
                CommentText = viewModel.CommentText,
                UserId = userId,
                CreatedBy = userName
            };

            var newComment = await _commentService.CreateReplyCommentAsync(repCommentId,comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = newComment.Id }, newComment);
        }
        // GET: api/Comments/{id}
        [HttpGet]
        [Route("Comment_By_Id")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // GET: api/Comments
        [HttpGet]
        [Route("Get_All_Of_Comment_From_Product")]
        public async Task<IActionResult> GetAllCommentsFromProduct(string productid)
        {
            var comments = await _commentService.GetAllCommentsFromProductAsync(productid);
            return Ok(comments);
        }

        // PUT: api/Comments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(string id, CreateReplyCommentModelView viewModel)
        {
            Guid userID = new Guid(User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            Comment existComment = await _commentService.GetCommentByIdAsync(id);
            if (existComment == null)
            {
                return NotFound();
            }
            if (existComment.UserId != userID) // Chỉ user tạo comment mới được cập nhật comment
            {
                return BadRequest();
            }
            existComment.CommentText = viewModel.CommentText;
            existComment.LastUpdatedBy = User.Identity?.Name;

            await _commentService.UpdateCommentAsync(existComment);
            return NoContent();
        }

        // DELETE: api/Comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            Guid userID = new Guid(User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);          
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment.UserId != userID) // Chỉ user tạo comment mới được delete comment
            {
                return BadRequest();
            }
            if (comment == null)
            {
                return NotFound();
            }

            await _commentService.DeleteCommentAsync(id);

            return NoContent();
        }
    }
}
