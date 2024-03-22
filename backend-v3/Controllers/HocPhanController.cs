using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HocPhanController : ControllerBase
    {
        private readonly IHocPhanService _services;
        public HocPhanController(IHocPhanService services)
        {
            _services = services;
        }

        [HttpGet]
        public Task<List<HocPhan>> GetAllHocPhan ([FromQuery] string? ThuMucId, string? keySearch)
        {
            try
            {
                return _services.GetAllHocPhan(ThuMucId, keySearch);
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
                return "OK";
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public Task<List<TheHoc>> GetTheHocById([FromQuery] string id)
        {
            try
            {
                return _services.GetHocPhanById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
