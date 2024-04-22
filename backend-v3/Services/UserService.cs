using backend_v3.Context;
using backend_v3.Controllers.common;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
namespace backend_v3.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMailService _mailService;
        private static readonly Random random = new Random();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public UserService(AppDbContext context, IMailService mailService )
        {
            _context = context;
            _mailService = mailService;
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

            var roleUser = _context.UserRoles.FirstOrDefault(x => x.UserId == id);
            var role = _context.Roles.FirstOrDefault(x => x.Id == roleUser.RoleId);
            var result = new UserDto
            {
                Id = data.Id,
                Ten = data.Ten,
                Username = data.Username,
                Email = data.Email,
                DiaChi = data.DiaChi,   
                GioiTinh = data.GioiTinh,
                NgaySinh = data.NgaySinh.Value.ToString("dd/MM/yyyy"),
                SoDienThoai = data.SoDienThoai,
                Role = role.Name,
            };
            return result;
        }

        public async Task<List<User>> GetAllUser()
        {
            var data = _context.Users.ToList();
            return data;

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

        public async Task UpdateAvatar(string id, string path)
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

                data.AnhDaiDien = path;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Users.Update(data);
                await _context.SaveChangesAsync(new CancellationToken());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginatedList<User>> GetPaginUser(UserRequest request)
        {
            var data = _context.Users.AsNoTracking();
            if (!string.IsNullOrEmpty(request.keySearch))
            {
                data = data.Where(x=> x.Ten.Contains(request.keySearch) || x.Username.Contains(request.keySearch) || x.GioiTinh.Contains(request.keySearch));
            }

            var result = PaginatedList<User>.Create(data, request.pageNumber, request.pageSize);

            return result;
        }

        public async Task<List<Role>> GetAllRole()
        {
            var data = _context.Roles.ToList();
            return data;
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public async Task<UserDto> AddUser(UserDto user)
        {
            DateTime newDate;
            DateTime.TryParseExact(user.NgaySinh, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out newDate);
            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = user.Username,
                Ten = user.Ten,
                NgaySinh = newDate,
                Email = user.Email,
                DiaChi = user.DiaChi,
                GioiTinh = user.GioiTinh,
                Password = GetMD5(user.Password),
                SoDienThoai = user.SoDienThoai,
                
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(new CancellationToken());

            var roleRequest = _context.Roles.FirstOrDefault(x => x.Name == user.Role);
            var newUserRole = new UserRole
            {
                RoleId = roleRequest.Id,
                UserId = newUser.Id,
            };
            _context.UserRoles.Add(newUserRole);
            await _context.SaveChangesAsync(new CancellationToken());

            return new UserDto
            {
                Id = newUser.Id,
                Email = user.Email,
                DiaChi = newUser.DiaChi,
                GioiTinh = newUser.GioiTinh,
                NgaySinh = newUser.NgaySinh.Value.ToString("dd/MM/yyyy"),
                Ten = newUser.Ten,
                SoDienThoai = newUser.SoDienThoai,
                Username = newUser.Username,
                Role = roleRequest.Name,
            };
        }

        public async Task EditInforUser_Admin(string id, UserDto userData)
        {
            try
            {
                DateTime newDate;
                DateTime.TryParseExact(userData.NgaySinh, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out newDate);
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
                data.NgaySinh = newDate;
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

        public async Task DeleteUser_Admin(string id)
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

            _context.Users.Remove(data);


            var RoleUser = _context.UserRoles.FirstOrDefault(x => x.UserId == id);
            _context.UserRoles.Remove(RoleUser);
            await _context.SaveChangesAsync(new CancellationToken());
        }

        public async Task ResetPassWord(string id)
        {
            if (id == null)
            {
                throw new Exception("Id không hợp lệ!");
            }
            var data = _context.Users.FirstOrDefault(x => x.Id == id);

            if (data == null)
            {
                throw new Exception("Không tìm thấy cán bộ!");
            }

            var passwordReset = GeneratePasswordResetCode(8);
            string? saltCode;
            var PasswordCode = GetMD5(passwordReset);
            if (PasswordCode == null)
            {
                throw new Exception("hash password không thành công!");
            }
            data.Password = PasswordCode;
            _context.Users.Update(data);
            await _context.SaveChangesAsync(new CancellationToken());

            var newMail = new MailData
            {
                EmailToId = data.Email!,
                EmailToName = data.Ten!,
                EmailSubject = "Reset password",
                EmailBody = $"Không cung cấp mật khẩu cho bất kỳ ai\n" +
                $"Mật khẩu mới của bạn cho tài khoản {data.Username} là:\n" +
                $"{passwordReset}",
            };
            await _mailService.SendMail(newMail);
        }

        public static string GeneratePasswordResetCode(int length)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Characters[random.Next(Characters.Length)];
            }
            return new string(result);
        }

        
    }
    
}
