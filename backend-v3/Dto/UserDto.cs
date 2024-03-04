namespace backend_v3.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; } = null!;
        public string? Ten { get; set; }
        public string? NgaySinh { get; set; }
        public string? GioiTinh { get; set; } = null!;
        public string? SoDienThoai { get; set; } = null!;
        public string? DiaChi { get; set; } = null!;
    }
}
