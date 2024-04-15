using backend_v3.Dto.Common;

namespace backend_v3.Dto
{
    public class UserDto 
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Ten { get; set; }
        public string? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? SoDienThoai { get; set; } 
        public string? DiaChi { get; set; }
        public string? Role { get; set; }

    }
}
