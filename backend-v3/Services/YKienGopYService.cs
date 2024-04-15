using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_v3.Services
{
    public class YKienGopYService : IYKienGopYService
    {
        private readonly AppDbContext _context;
        public YKienGopYService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PaginatedList<YKienGopYDo>> GetPaginYKienGopY(YKienGopYRequest request)
        {
            var data = _context.YkienGopYs.AsNoTracking();

            if(!string.IsNullOrEmpty(request.UserId))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x=> x.Username == request.UserId);
                data = data.Where(x => x.UserId == user.Id);
            }

            if (!string.IsNullOrEmpty(request.keySearch))
            {
                data = data.Where(x => x.NoiDung == request.keySearch);
            }

            var users = _context.Users.AsNoTracking();

            var resultData = data.Select(x => new YKienGopYDo
            {
                Id = x.Id,
                UserId = x.UserId,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
                NoiDung = x.NoiDung,
                PhanHoi = x.PhanHoi,
                Username = users.Where(y => y.Id == x.UserId).Select(y => y.Username).FirstOrDefault()
            }).AsNoTracking();

            var result = PaginatedList<YKienGopYDo>.Create(resultData, request.pageNumber, request.pageSize);

            return result;
        }

        public async Task<PaginatedList<YKienGopYDo>> GetYKienGopY(YKienGopYRequest request)
        {
            var data = _context.YkienGopYs.AsNoTracking();

            if (!string.IsNullOrEmpty(request.UserId))
            {
                data = data.Where(x => x.UserId == request.UserId);
            }

            var users = _context.Users.AsNoTracking();

            var resultData = data.Select(x => new YKienGopYDo
            {
                Id = x.Id,
                UserId = x.UserId,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
                NoiDung = x.NoiDung,
                PhanHoi = x.PhanHoi,
                Username = users.Where(y => y.Id == x.UserId).Select(y => y.Username).FirstOrDefault()
            }).AsNoTracking();

            var result = PaginatedList<YKienGopYDo>.Create(resultData, request.pageNumber, request.pageSize);

            return result;
        }


        public async Task<YkienGopY> ThemYKien(YKienGopYDo data)
        {
            var newYKien = new YkienGopY
            {
                Id = Guid.NewGuid().ToString(),
                NoiDung = data.NoiDung,
                UserId = data.UserId,
                Created = DateTime.Now,
                CreatedBy = data.CreatedBy,
                LastModified = DateTime.Now,
                LastModifiedBy = data.LastModifiedBy,   
            };

            _context.YkienGopYs.Add(newYKien);
            await _context.SaveChangesAsync(new CancellationToken());
            return newYKien;
        }

        public async Task<YKienGopYDo> GetYKienById(string id)
        {
            var data = _context.YkienGopYs.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Không có dữ liệu!");
            }

            return new YKienGopYDo
            {
                Id = id,
                NoiDung = data.NoiDung,
                PhanHoi = data.PhanHoi,
                Created = data.Created,
                CreatedBy= data.CreatedBy,
                LastModified= data.LastModified,
                LastModifiedBy= data.LastModifiedBy,
                Username = _context.Users.Where(y => y.Id == data.UserId).Select(y => y.Username).FirstOrDefault()
            };
        }

        public async Task DeleteYKienById(string id)
        {
            var data = _context.YkienGopYs.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Không có dữ liệu!");
            }

            _context.YkienGopYs.Remove(data);
            await _context.SaveChangesAsync(new CancellationToken());
        }

        public async Task PhanHoi(string id, string phanhoi)
        {
            var data = _context.YkienGopYs.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Không có dữ liệu!");
            }

            data.PhanHoi = phanhoi;
            _context.YkienGopYs.Update(data);
            await _context.SaveChangesAsync(new CancellationToken());
        }
    }
}
