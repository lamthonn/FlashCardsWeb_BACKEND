﻿using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

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

        [HttpDelete]
        public Task DeleteHocPhan( string id)
        {
            try
            {
                return _services.DeleteHocPhan(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        [HttpPut]
        public Task EditHocPhan( string id, HocPhanParams data)
        {
            try
            {
                return _services.EditHocPhan(id, data);
            }
            catch (Exception ex)
            {
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
    }
}
