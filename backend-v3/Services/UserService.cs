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

        public async Task  EditInforUser(string id, UserDto userData)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Không tìm thấy id người dùng");
                }

                var data = _context.Users.FirstOrDefault(x => x.Id == id);
                if (data == null)
                {
                    throw new Exception("User không tồn tại!");
                }

                // Cập nhật thông tin của người dùng
                data.Ten = userData.Ten;
                data.Username = userData.Username;
                data.Email = userData.Email;
                data.DiaChi = userData.DiaChi;
                data.GioiTinh = userData.GioiTinh;
                data.NgaySinh = DateTime.ParseExact(userData.NgaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                data.SoDienThoai = userData.SoDienThoai;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Users.Update(data);
                await _context.SaveChangesAsync(new CancellationToken());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
