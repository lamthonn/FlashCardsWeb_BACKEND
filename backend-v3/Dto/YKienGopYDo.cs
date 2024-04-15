namespace backend_v3.Dto
{
    public class YKienGopYDo
    {
        public string Id { get; set; }
        public string? NoiDung { get; set; }
        public string? PhanHoi { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
