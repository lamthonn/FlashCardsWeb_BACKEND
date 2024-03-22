using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_v3.Services
{
    public class HocPhanService : IHocPhanService
    {
        private readonly AppDbContext _context;
        public HocPhanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HocPhan>> GetAllHocPhan([FromQuery] string? ThuMucId, string? keySearch)
        {
            var data = _context.HocPhans.AsNoTracking();
            if (!string.IsNullOrEmpty(ThuMucId))
            {
                data = data.Where(x => x.ThuMucId == ThuMucId);
            }
            if (!string.IsNullOrEmpty(keySearch))
            {
                data = data.Where(x => x.TieuDe == keySearch);
            }
            var result = await data.ToListAsync();
            return result;
        }

        public async Task<List<TheHoc>> GetHocPhanById([FromQuery] string id)
        {
            try
            {
                var ListTheHoc = _context.TheHocs.Where(x => x.HocPhanId == id);
                var result = await ListTheHoc.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ThemHocPhan([FromBody] HocPhanParams _params)
        {
            var newhocphan = new HocPhan
            {
                Id = Guid.NewGuid().ToString(),
                TieuDe = _params.hocphan.TieuDe!,
                MoTa = _params.hocphan.MoTa,
                NgayTao = ConvertToDatetime(_params.hocphan.NgayTao!),
                NgaySua = ConvertToDatetime(_params.hocphan.NgaySua!),
                TrangThai = _params.hocphan.TrangThai,
                SoTheHoc = _params.theHocs.Count(),
                ThuMucId = _params.hocphan.ThuMucId!,
                UserId = _params.hocphan.UserId!,
                Created = DateTime.Now,
                CreatedBy = _params.hocphan.CreatedBy,
                LastModified = DateTime.Now,
                LastModifiedBy = _params.hocphan.LastModifiedBy,
            };
            _context.HocPhans.Add(newhocphan);
            await _context.SaveChangesAsync(new CancellationToken());

            var newListTheHoc = _params.theHocs.Select(x => new TheHoc
            {
                Id = Guid.NewGuid().ToString(),
                NgonNgu1 = x.NgonNgu1!,
                NgonNgu2 = x.NgonNgu2!,
                HinhAnh = x.HinhAnh!,
                IsKnow = false,
                HocPhanId = newhocphan.Id,
                Created = DateTime.Now,
                CreatedBy= newhocphan.CreatedBy,
                LastModified = DateTime.Now,
                LastModifiedBy= newhocphan.LastModifiedBy,
            });

            _context.TheHocs.AddRange(newListTheHoc);
            await _context.SaveChangesAsync(new CancellationToken());
        }

        private DateTime ConvertToDatetime(string timeStamp)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timeStamp));
            // Get DateTime object
            DateTime dateTime = dateTimeOffset.DateTime;
            return dateTime;
        }
    }
}
