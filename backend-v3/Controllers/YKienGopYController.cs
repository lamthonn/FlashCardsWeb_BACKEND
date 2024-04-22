using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using backend_v3.Seriloger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Ilogger = Serilog.ILogger;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class YKienGopYController : ControllerBase
    {
        private readonly IYKienGopYService _services;
        private readonly AppDbContext _context;
        private readonly Ilogger _logger;
        private readonly LoggingCommon _loggingCommon;
        public YKienGopYController(IYKienGopYService services, Ilogger logger, AppDbContext context)
        {
            _services = services;
            _context = context;
            _logger = logger;
            _loggingCommon = new LoggingCommon(_logger, _context);
        }

        [HttpGet]
        public Task<PaginatedList<YKienGopYDo>> GetAllYKien([FromQuery] YKienGopYRequest request)
        {
            try
            {
                return _services.GetPaginYKienGopY(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public Task<PaginatedList<YKienGopYDo>> GetYKienGopY([FromQuery] YKienGopYRequest request)
        {
            try
            {
                return _services.GetYKienGopY(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public Task<YKienGopYDo> GetYKienById([FromQuery] string id)
        {
            try
            {
                return _services.GetYKienById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpDelete]
        public Task DeleteYKienById([FromQuery] string id)
        {
            var yKien = _context.YkienGopYs.FirstOrDefault(x => x.Id == id);
            try
            {
                _loggingCommon.AddLoggingInformation(
                    $"Xóa ý kiến đóng góp {yKien.NoiDung}",
                    yKien.UserId,
                    LoggingType.NHAT_KY_THAO_TAC_NGUOI_DUNG
                );
                return _services.DeleteYKienById(id);
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingInformation(
                    $"Lỗi xóa ý kiến đóng góp: {ex.Message}",
                    yKien.UserId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public Task<YkienGopY> ThemYKien(YKienGopYDo data)
        {
            try
            {
                _loggingCommon.AddLoggingInformation(
                    $"Thêm ý kiến đóng góp {data.NoiDung}",
                    data.UserId,
                    LoggingType.NHAT_KY_THAO_TAC_NGUOI_DUNG
                );
                return _services.ThemYKien(data);
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingInformation(
                    $"Lỗi thêm ý kiến đóng góp: {ex.Message}",
                    data.UserId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                throw new Exception(ex.Message);
            }
        }
        [HttpPut]
        public Task PhanHoi(string id, string data, string? userId)
        {
            var yKien = _context.YkienGopYs.FirstOrDefault(x=> x.Id == id); 
            try
            {
                _loggingCommon.AddLoggingInformation(
                    $"Phản hồi ý kiến đóng góp {yKien.NoiDung}",
                    userId,
                    LoggingType.NHAT_KY_THAO_TAC_QUAN_TRI
                );
                return _services.PhanHoi(id,data);
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingInformation(
                    $"Lỗi thêm ý kiến đóng góp: {ex.Message}",
                    userId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                throw new Exception(ex.Message);
            }
        }
    }
}
