using backend_v3.Dto;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_v3.Interfaces
{
    public interface IHocPhanService
    {
        public Task<List<HocPhan>> GetAllHocPhan([FromQuery] string? ThuMucId, string? keySearch);
        public Task ThemHocPhan([FromBody] HocPhanParams _params);
        public Task<List<TheHoc>> GetHocPhanById (string id);
        public Task<HocPhanDto> GetHocPhanName (string id);
        public Task DeleteHocPhan (string id);
        public Task EditHocPhan (string id, HocPhanParams data);
        public Task<List<TheHoc>> GetRandom(string id);
    }
}
