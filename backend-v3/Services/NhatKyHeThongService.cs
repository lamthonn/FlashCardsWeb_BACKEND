using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_v3.Services
{
    public class NhatKyHeThongService : INhatKyHeThongService
    {
        private readonly AppDbContext _context;
        public NhatKyHeThongService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PaginatedList<NhatKyHoatDong>> GetpaginNhatKy(NhatKyHoatDongRequest request)
        {
            var user = _context.Users.FirstOrDefault(x=> x.Id == request.UserId);
            var data = _context.NhatKyHoatDongs.Where(x => x.UserName == user.Username).OrderByDescending(y=> y.TimeStamp);

            var result = PaginatedList<NhatKyHoatDong>.Create(data, request.pageNumber, request.pageSize);

            return result;
        }
    }
}
