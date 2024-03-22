using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ThuMucController : ControllerBase
    {
        private readonly IThuMucService _services;
        public ThuMucController(IThuMucService services)
        {
            _services = services;
        }

        [HttpGet] 
        public  Task<List<ThuMuc>> GetAllThuMuc ([FromQuery] Params _params)
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
        public Task<ThuMuc> ThemThuMuc(ThuMucDto thumuc)
        {
            try
            {
                return _services.ThemThuMuc(thumuc);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
