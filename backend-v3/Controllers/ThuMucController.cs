using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Interfaces;
using backend_v3.Models;
using backend_v3.Seriloger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ilogger = Serilog.ILogger;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ThuMucController : ControllerBase
    {
        private readonly IThuMucService _services;
        private readonly AppDbContext _context;
        private readonly Ilogger _logger;
        private readonly LoggingCommon _loggingCommon;
        public ThuMucController(IThuMucService services, Ilogger logger, AppDbContext context)
        {
            _services = services;
            _context = context;
            _logger = logger;
            _loggingCommon = new LoggingCommon(_logger, _context);
        }

        [HttpGet] 
        public  Task<List<ThuMuc>> GetAllThuMuc ([FromQuery] ThuMucRequest _params)
        {
            try
            {
                return _services.GetAllThuMuc(_params);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ThuMuc> ThemThuMuc(ThuMucDto thumuc)
        {
            try
            {
                var result = await _services.ThemThuMuc(thumuc);
                _loggingCommon.AddLoggingInformation(
                    $"Thêm thư mục {thumuc.TieuDe} #{thumuc.Id}",
                    thumuc.UserId,
                    LoggingType.NHAT_KY_THAO_TAC_NGUOI_DUNG
                );
                return result;
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                    $"Lỗi thêm thư mục: {ex.Message}",
                    thumuc.UserId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                throw new Exception(ex.Message);
            }
        }

    }
}
