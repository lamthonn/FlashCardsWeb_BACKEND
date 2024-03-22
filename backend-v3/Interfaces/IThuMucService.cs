using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_v3.Interfaces
{
    public interface IThuMucService
    {
        public Task<List<ThuMuc>> GetAllThuMuc([FromQuery] Params _params);
        public Task<ThuMuc> ThemThuMuc(ThuMucDto thumuc);
    }
}
