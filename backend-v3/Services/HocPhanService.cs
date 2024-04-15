using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend_v3.Services
{
    public class HocPhanService : IHocPhanService
    {
        private readonly AppDbContext _context;
        public HocPhanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteHocPhan(string id)
        {
            var AllTheHocs = _context.TheHocs.Where(h => h.Id == id);
            var hocPhan = _context.HocPhans.FirstOrDefault(h => h.Id == id);

            if (hocPhan == null)
            {
                throw new Exception("Không tìm thấy học phần!");
            }

            _context.TheHocs.RemoveRange(AllTheHocs);
            _context.HocPhans.Remove(hocPhan);
            await _context.SaveChangesAsync();

        }

        public async Task EditHocPhan(string id, HocPhanParams datas)
        {
            var AllTheHocs = _context.TheHocs.Where(h => h.HocPhanId == id);
            var hocPhan = _context.HocPhans.FirstOrDefault(h => h.Id == id);

            if (hocPhan == null)
            {
                throw new Exception("Không tìm thấy học phần!");
            }

            hocPhan.Id = id;
            hocPhan.TieuDe = datas.hocphan.TieuDe;
            hocPhan.MoTa = datas.hocphan.MoTa;
            hocPhan.ThuMucId = datas.hocphan.ThuMucId;
            hocPhan.LastModified = DateTime.Now;
            hocPhan.LastModifiedBy = datas.hocphan.LastModifiedBy;
            _context.HocPhans.Update(hocPhan);
            await _context.SaveChangesAsync();


            //trường hợp không thêm, không xóa
            if(AllTheHocs.Count() == datas.theHocs.Count)
            {
                foreach (var item in datas.theHocs)
                {
                    var thehoc = AllTheHocs.FirstOrDefault(x => x.Id == item.Id);
                    if (thehoc != null)
                    {
                        thehoc.ThuatNgu = item.ThuatNgu;
                        thehoc.GiaiThich = item.GiaiThich;
                        thehoc.NgonNgu1 = item.NgonNgu1;
                        thehoc.NgonNgu2 = item.NgonNgu2;

                        _context.TheHocs.Update(thehoc);
                        await _context.SaveChangesAsync();
                    }
                }
                await _context.SaveChangesAsync();
            }
            //trường hợp thêm
            if (AllTheHocs.Count() < datas.theHocs.Count)
            {
                foreach (var item in datas.theHocs)
                {
                    var thehoc = AllTheHocs.FirstOrDefault(x => x.Id == item.Id);
                    if(thehoc == null)
                    {
                        var newTheHoc = new TheHoc
                        {
                            Id = Guid.NewGuid().ToString(),
                            NgonNgu1 = item.NgonNgu1,
                            NgonNgu2 = item.NgonNgu2,
                            ThuatNgu = item.ThuatNgu,
                            GiaiThich = item.GiaiThich,
                            HocPhanId = item.HocPhanId,
                            IsKnow = false,
                            Created = item.Created,
                            CreatedBy = item.CreatedBy,
                            LastModified = item.LastModified,
                            LastModifiedBy = item.LastModifiedBy,
                        };
                        _context.TheHocs.Add(newTheHoc);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            //trường hợp xóa
            if (AllTheHocs.Count() > datas.theHocs.Count)
            {
                //hỏi chatgpt
                foreach (var item in AllTheHocs)
                {
                    var thehoc = datas.theHocs.FirstOrDefault(x => x.Id == item.Id);
                    if (thehoc == null)
                    {
                        var dataRemove = _context.TheHocs.FirstOrDefault(x => x.Id == item.Id);
                        _context.TheHocs.Remove(dataRemove);
                        await _context.SaveChangesAsync();
                    }
                }
            }
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

        public async Task<HocPhanDto> GetHocPhanName(string id)
        {
            var data = _context.HocPhans.FirstOrDefault(x => x.Id == id);
            if(data != null)
            {
                var result = new HocPhanDto
                {
                    Id = data.Id,   
                    TieuDe = data.TieuDe,
                    MoTa = data.MoTa,
                    Created = data.Created,
                    CreatedBy = data.CreatedBy,
                    LastModified = data.LastModified,
                    LastModifiedBy = data.LastModifiedBy,
                    ThuMucId = data.ThuMucId,
                    TrangThai = data.TrangThai  ,
                    UserId = data.UserId    
                };

                return result;
            }

            throw new Exception("Không có dữ liệu!!");
        }

        public async Task<List<TheHoc>> GetRandom(string id)
        {
            var hocphan = _context.TheHocs.Where(x => x.HocPhanId == id);
            var data = await hocphan.OrderBy(x => Guid.NewGuid()).Take(10).AsNoTracking().ToListAsync();
            // Duplicate the list
            //var duplicatedData = new List<TheHoc>(data);

            //// Shuffle the duplicated list
            var rnd = new Random();
            int n = data.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = data[k];
                data[k] = data[n];
                data[n] = value;
            }

            return data;
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
                ThuatNgu = x.ThuatNgu,
                GiaiThich = x.GiaiThich,
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
