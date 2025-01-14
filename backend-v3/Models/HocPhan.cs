﻿using backend_v3.Models.common;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_v3.Models
{
    public class HocPhan : BaseModel
    {
        public string Id { get; set; }
        public string TieuDe { get; set; }
        public string? MoTa { get; set; } = null!;
        public DateTime? NgayTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string? TrangThai { get; set; }
        public int? SoTheHoc { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        public string ThuMucId { get; set; }
        [ForeignKey(nameof(ThuMucId))]
        public virtual ThuMuc? ThuMuc { get; set; }
        public virtual List<TheHoc>? TheHocs { get; set; }
    }
}
