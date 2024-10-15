using FUExchange.Contract.Repositories.Entity;
using FUExchange.Services.Service;
using Microsoft.AspNetCore.Mvc;
using FUExchange.ModelViews.CommentModelViews;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;
using FUExchange.Core.Base;

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

            await _commentService.CreateComment(viewModel);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Create successfully."));
        }

        [HttpPost]
        [Route("Reply_Comment")]
        public async Task<IActionResult> CreateReplyComment(string repCommentId, CreateReplyCommentModelView viewModel)
        {
            await _commentService.CreateReplyComment(repCommentId, viewModel);

            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Create successfully."));
        }


        // GET: api/Comments/{id}
        [HttpGet]
        [Route("Comment_By_Id")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            try
            {
                var comment = await _commentService.GetCommentById(id);
                return Ok(new BaseResponseModel(
                                     StatusCodes.Status200OK,
                                     ResponseCodeConstants.SUCCESS,
                                     comment));
            }
            catch (BaseException.ErrorException ex)
            {
                return Ok(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }
        }

        // GET: api/Comments
        [HttpGet]
        [Route("Get_All_Of_Comment_From_Product")]
        public async Task<IActionResult> GetAllCommentsFromProduct(string productid, int pageIndex = 1, int pageSize = 2)
        {
            try
            {
                var comments = await _commentService.GetAllCommentsFromProduct(productid, pageIndex, pageSize);
                return Ok(new BaseResponseModel(
                        StatusCodes.Status200OK,
                        ResponseCodeConstants.SUCCESS,
                        comments));
            }
            catch (BaseException.ErrorException ex)
            {
                return Ok(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }
        }

        // PUT: api/Comments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(string id, CreateCommentModelViews viewModel)
        {
            try
            {
                await _commentService.UpdateComment(id, viewModel);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Update successfully"));
            }          
            catch (BaseException.ErrorException ex)
            {
                return Ok(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }
        }

        // DELETE: api/Comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            try
            {
                await _commentService.DeleteComment(id);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Delete successfully"));
            }
            catch (BaseException.ErrorException ex)
            {
                return Ok(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }
        }
    }
}
