using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Interfaces;
using backend_v3.Models;
using backend_v3.Seriloger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Ilogger = Serilog.ILogger;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HocPhanController : ControllerBase
    {
        private readonly IHocPhanService _services;
        private readonly AppDbContext _context;
        private readonly Ilogger _logger;
        private readonly LoggingCommon _loggingCommon;
        public HocPhanController(IHocPhanService services, Ilogger logger, AppDbContext context)
        {
            _services = services;
            _context = context;
            _logger = logger;
            _loggingCommon = new LoggingCommon(_logger, _context);
        }

        [HttpGet]
        public Task<List<HocPhan>> GetAllHocPhan ([FromQuery] string? ThuMucId, string? keySearch, string? userId, string? soft)
        {
            try
            {
                return _services.GetAllHocPhan(ThuMucId, keySearch, userId, soft);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<string> ThemHocPhan ([FromBody] HocPhanParams _params)
        {
            try
            {
                await _services.ThemHocPhan(_params);
                _loggingCommon.AddLoggingInformation(
                    $"Thêm Học phần {_params.hocphan.TieuDe} #{_params.hocphan.Id}",
                    _params.UserId,
                    LoggingType.NHAT_KY_THAO_TAC_NGUOI_DUNG
                );
                return "OK";
            }catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                    $"Lỗi thêm Học phần: {ex.Message}",
                    _params.UserId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public Task<HocPhanDto> HocPhanName(string id)
        {
            try
            {
                return _services.GetHocPhanName(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public Task<List<TheHoc>> GetTheHocById([FromQuery] string id, string? soft)
        {
            try
            {
                return _services.GetHocPhanById(id, soft);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        public Task DeleteHocPhan(string id, string? userId)
        {
            try
            {
                var hocphan = _context.HocPhans.FirstOrDefault(h => h.Id == id);
                _loggingCommon.AddLoggingInformation(
                    $"Xóa Học phần {hocphan.TieuDe} #{id}",
                    userId,
                    LoggingType.NHAT_KY_THAO_TAC_NGUOI_DUNG
                );
                return _services.DeleteHocPhan(id);
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                   $"Lỗi xóa Học phần: {ex.Message}",
                   userId,
                   LoggingType.NHAT_KY_LOI_PHAT_SINH
               );
                throw new Exception(ex.Message);
            }
        }
        
        [HttpPut]
        public Task EditHocPhan( string id, HocPhanParams data)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == data.hocphan!.UserId);
            try
            {
                _loggingCommon.AddLoggingInformation(
                    $"Sửa Học phần {data.hocphan.TieuDe} #{id}",
                    user.Id,
                    LoggingType.NHAT_KY_THAO_TAC_NGUOI_DUNG
                );
                return _services.EditHocPhan(id, data);
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                   $"Lỗi sửa Học phần: {ex.Message}",
                   user.Id,
                   LoggingType.NHAT_KY_LOI_PHAT_SINH
               );
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public Task<List<TheHoc>> GetRandom(string id)
        {
            try
            {
                return _services.GetRandom(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        
        [HttpGet]
        public Task<object> GetCardForLearn(string id)
        {
            try
            {
                return _services.GetCardForLearn(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public Task<object> GetListCardForTest(string id)
        {
            try
            {
                return _services.GetListCardForTest(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public Task<bool> CheckResult1([FromBody] InputCheckTest input)
        {
            try
            {
                return _services.CheckResult1(input.Id, input.Answer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
