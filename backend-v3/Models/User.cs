using System.ComponentModel.DataAnnotations;

namespace backend_v3.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; }
        public string Ten { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; } = null!;
        public string SoDienThoai { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
    }
}
