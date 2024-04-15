namespace backend_v3.Dto
{
    public class TheHocDto
    {
        public string Id { get; set; }
        public string? NgonNgu1 { get; set; }
        public string? NgonNgu2 { get; set; }
        public byte[]? HinhAnh { get; set; }
        public string? HocPhanId { get; set; }
        public string? ThuatNgu { get; set; }
        public string? GiaiThich { get; set; }
        public bool? IsKnow { get; set; } = false;
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
