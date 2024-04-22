namespace backend_v3.Dto
{
    public class ThuMucDto
    {
        public string Id { get; set; }
        public string? TieuDe { get; set; }
        public string? MoTa { get; set; }
        public string? UserId { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
