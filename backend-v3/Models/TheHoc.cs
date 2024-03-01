namespace backend_v3.Models
{
    public class TheHoc
    {
        public string Id { get; set; }
        public string NgonNgu1 { get; set; }
        public string NgonNgu2 { get; set; }
        public byte[]? HinhAnh { get; set; }
        public string HocPhanId { get; set; }
        public virtual HocPhan? HocPhan { get; set; }
    }
}
