using FUExchange.Contract.Services.Interface;
using FUExchange.ModelViews.ReportModelViews;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FUExchange.Core.Base;
namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")] // Chỉ cho phép truy cập bởi admin
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports(int pageIndex = 1, int pageSize = 10)
        {
            var paginatedReports = await _reportService.GetAllReports(pageIndex, pageSize);

            // Sử dụng thuộc tính Items để truy cập danh sách các báo cáo
            var reportResponseModels = paginatedReports.Items.Select(report => new ReportResponseModel
            {
                Id = report.Id.ToString(),
                Reason = report.Reason,
                Status = report.Status
            });

            return Ok(new BaseResponse<IEnumerable<ReportResponseModel>>(
                statusCode: StatusCodeHelper.OK,
                code: StatusCodeHelper.OK.ToString(),
                data: reportResponseModels
            ));
        }
        // Get Report by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(string id)
        {
            try
            {
                var report = await _reportService.GetReportById(id);
                return Ok(new BaseResponse<ReportResponseModel>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: report
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }
        }

        // Create Report
        public static class ResponseCodeConstants
        {
            public const string NOT_FOUND = "NOT_FOUND";
            public const string BAD_REQUEST = "BAD_REQUEST"; 
        }
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportRequestModel reportRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _reportService.CreateReport(reportRequest);
                return Ok(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: "Report created successfully."
                ));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.BAD_REQUEST,
                    data: ex.Message
                ));
            }
        }

        // Update Report
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(string id, [FromBody] ReportRequestModel reportRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _reportService.UpdateReport(id, reportRequest);
                return Ok(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: "Report updated successfully."
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }
        }

        // Delete Report
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(string id)
        {
            try
            {
                await _reportService.DeleteReport(id);
                return Ok(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: "Report deleted successfully."
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }
        }

        // Get Reports by Reason
        [HttpGet("search")]
        public async Task<IActionResult> GetReportsByReason([FromQuery] string reason)
        {
            var reports = await _reportService.GetReportsByReason(reason);
            return Ok(new BaseResponse<IEnumerable<ReportResponseModel>>(
                statusCode: StatusCodeHelper.OK,
                code: StatusCodeHelper.OK.ToString(),
                data: reports
            ));
        }
        //kiểm tra trạng thái của report
        [HttpGet("{id}/status")]
        public async Task<IActionResult> CheckReportStatus(string id)
        {
            try
            {
                var status = await _reportService.CheckReportStatus(id);
                return Ok(new BaseResponse<bool>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: status
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }
        }


    }
}
