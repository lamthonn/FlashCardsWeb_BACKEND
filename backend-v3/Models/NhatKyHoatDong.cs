namespace backend_v3.Models
{
    public class NhatKyHoatDong
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? IP { get; set; }
        public string? Message { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Level { get; set; }
        public string? PhanLoai { get; set; }
    }
}
