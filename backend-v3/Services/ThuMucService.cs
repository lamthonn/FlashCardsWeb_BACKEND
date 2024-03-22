using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend_v3.Services
{
    public class ThuMucService : IThuMucService
    {
        private readonly AppDbContext _context;
        public ThuMucService(AppDbContext context)
        {
            _context = context;
        }
        public Task<List<ThuMuc>> GetAllThuMuc([FromQuery] Params _params)
        {
            var data =  _context.ThuMucs.AsNoTracking();
            if (!string.IsNullOrEmpty(_params.keySearch))
            {
                data = data.Where(x => x.TieuDe.Contains(_params.keySearch));
            }

            var result = data.ToList();
            return Task.FromResult(result);
        }

        public async Task<ThuMuc> ThemThuMuc(ThuMucDto thumuc)
        {
            var data = new ThuMuc
            {
                Id = Guid.NewGuid().ToString(),
                TieuDe = thumuc.TieuDe,
                MoTa = thumuc.MoTa,
                Created = DateTime.Now,
                CreatedBy = thumuc.CreatedBy,
                LastModified = DateTime.Now,
                LastModifiedBy = thumuc.LastModifiedBy,
            };
            _context.ThuMucs.Add(data);
            await _context.SaveChangesAsync(new CancellationToken());
            return data;
        }
    }
}
