using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using System.Globalization;

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

        public async Task<UserDto> EditInforUser(string id, UserDto userData)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Không tìm thấy id người dùng");
            }

            var userToUpdate = _context.Users.FirstOrDefault(x => x.Id == id);
            if (userToUpdate == null)
            {
                throw new Exception("User không tồn tại!");
            }

            // Cập nhật thông tin của người dùng
            userToUpdate.Ten = userData.Ten;
            userToUpdate.Username = userData.Username;
            userToUpdate.Email = userData.Email;
            userToUpdate.DiaChi = userData.DiaChi;
            userToUpdate.GioiTinh = userData.GioiTinh;
            userToUpdate.NgaySinh = DateTime.ParseExact(userData.NgaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            userToUpdate.SoDienThoai = userData.SoDienThoai;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Users.Update(userToUpdate);
            await _context.SaveChangesAsync(new CancellationToken());

            // Trả về thông tin người dùng đã được cập nhật
            return userData;
        }
    }
}
