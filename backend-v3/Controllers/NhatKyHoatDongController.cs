using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NhatKyHoatDongController : ControllerBase
    {
        private readonly INhatKyHeThongService _services;
        public NhatKyHoatDongController(INhatKyHeThongService services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<PaginatedList<NhatKyHoatDong>> GetPaginLog([FromQuery]NhatKyHoatDongRequest request)
        {
            try
            {
                return await _services.GetpaginNhatKy(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
