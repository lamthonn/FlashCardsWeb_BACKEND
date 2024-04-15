using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class YKienGopYController : ControllerBase
    {
        private readonly IYKienGopYService _services;
        public YKienGopYController(IYKienGopYService services)
        {
            _services = services;
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
            try
            {
                return _services.DeleteYKienById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public Task<YkienGopY> ThemYKien(YKienGopYDo data)
        {
            try
            {
                return _services.ThemYKien(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPut]
        public Task PhanHoi(string id, string data)
        {
            try
            {
                return _services.PhanHoi(id,data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
