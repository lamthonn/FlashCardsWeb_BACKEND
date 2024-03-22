namespace backend_v3.Dto
{
    public class HocPhanDto
    {
        public string Id { get; set; }
        public string? TieuDe { get; set; }
        public string? MoTa { get; set; } = null!;
        public string? NgayTao { get; set; }
        public string? NgaySua { get; set; }
        public string? TrangThai { get; set; }
        public string? UserId { get; set; }
        public string? ThuMucId { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
