using backend_v3.Models.common;

namespace backend_v3.Models
{
    public class ThuMuc : BaseModel
    { 
        public string Id { get; set; }
        public string TieuDe { get; set; }
        public string? MoTa { get; set; }
        public string? UserId { get; set; }
        public virtual List<HocPhan>? HocPhans { get; set; }
    }
}
