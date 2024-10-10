﻿using FUExchange.Contract.Repositories.Entity;
using FUExchange.Services.Service;
using Microsoft.AspNetCore.Mvc;
using FUExchange.ModelViews.CommentModelViews;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;

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
            var comment = await _commentService.GetCommentById(id);
            return Ok(new BaseResponseModel(
                                 StatusCodes.Status200OK,
                                 ResponseCodeConstants.SUCCESS,
                                 comment));
        }

        // GET: api/Comments
        [HttpGet]
        [Route("Get_All_Of_Comment_From_Product")]
        public async Task<IActionResult> GetAllCommentsFromProduct(string productid, int pageIndex = 1, int pageSize = 2)
        {
            var comments = await _commentService.GetAllCommentsFromProduct(productid, pageIndex, pageSize);
            return Ok(new BaseResponseModel(
                    StatusCodes.Status200OK,
                    ResponseCodeConstants.SUCCESS,
                    comments));
        }

        // PUT: api/Comments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(string id, CreateCommentModelViews viewModel)
        {

            await _commentService.UpdateComment(id, viewModel);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Update successfully"));

        }

        // DELETE: api/Comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            await _commentService.DeleteComment(id);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Delete successfully"));
        }
    }
}
