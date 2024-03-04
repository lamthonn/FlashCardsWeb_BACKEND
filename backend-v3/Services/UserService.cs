using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;

namespace backend_v3.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserDto> GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Id không hợp lệ!");
            }

            var data = _context.Users.FirstOrDefault(x => x.Id == id); 
            if (data == null)
            {
                throw new Exception("User không tồn tại!");
            }

            var result = new UserDto
            {
                Id = data.Id,
                Ten = data.Ten,
                Username = data.Username,
                Email = data.Email,
                DiaChi = data.DiaChi,   
                GioiTinh = data.GioiTinh,
                NgaySinh = data.NgaySinh.Value.ToString("dd/MM/yyyy"),
                SoDienThoai = data.SoDienThoai
            };
            return result;
        }
    }
}
