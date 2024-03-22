using backend_v3.Models.common;

namespace backend_v3.Models
{
    public class TheHoc : BaseModel
    {
        public string Id { get; set; }
        public string NgonNgu1 { get; set; }
        public string NgonNgu2 { get; set; }
        public byte[]? HinhAnh { get; set; }
        public string HocPhanId { get; set; }
        public bool? IsKnow { get; set; } = false;
        public virtual HocPhan? HocPhan { get; set; }
    }
}
