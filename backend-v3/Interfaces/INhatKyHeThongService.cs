using backend_v3.Dto;
using backend_v3.Models;

namespace backend_v3.Interfaces
{
    public interface INhatKyHeThongService
    {
        public Task<PaginatedList<NhatKyHoatDong>> GetpaginNhatKy(NhatKyHoatDongRequest request);
    }
}
