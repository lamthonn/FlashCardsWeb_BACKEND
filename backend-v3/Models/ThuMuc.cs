namespace backend_v3.Models
{
    public class ThuMuc
    {
        public string Id { get; set; }
        public string TieuDe { get; set; }
        public string? MoTa { get; set; }
        public virtual List<HocPhan>? HocPhans { get; set; }
    }
}
