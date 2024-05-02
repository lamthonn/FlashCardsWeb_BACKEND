using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backend_v3.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        public AddUserRole Register(AddUserRole user)
        {
            try
            {
                var userConflic = _context.Users.FirstOrDefault(x => x.Username == user.Username);
                if (userConflic == null)
                {
                    string hashPassword = GetMD5(user.Password);
                    var newUser = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Ten = user.Ten,
                        Username = user.Username,
                        Password = hashPassword,
                        Email = "Chưa có thông tin!",
                        DiaChi = "Chưa có thông tin!",
                        GioiTinh = "Chưa có thông tin!",
                        SoDienThoai = "Chưa có thông tin!",
                        NgaySinh = DateTime.Now,
                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    //đăng ký role
                    var addRoles = new List<UserRole>();
                    foreach (var role in user.RoleIds)
                    {
                        var userRole = new UserRole();
                        userRole.RoleId = role;
                        userRole.UserId = newUser.Id;
                        addRoles.Add(userRole);
                    }
                    _context.UserRoles.AddRange(addRoles);
                    _context.SaveChanges();

                    return new AddUserRole
                    {
                        Id = newUser.Id,
                        Ten = newUser.Ten,
                        Username = newUser.Username,
                        RoleIds = addRoles.Select(r => r.RoleId).ToList()
                    };
                }
                else
                {
                    throw new Exception("Tài khoản đã tồn tại");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Role AddRole(Role role)
        {
            var newRole = new Role
            {
                Name = role.Name,
                Description = role.Description,
            };
            _context.Roles.Add(newRole);
            _context.SaveChanges();
            return newRole;
        }


        public string Login(LoginRequest loginRequest)
        {
            if(loginRequest.Username != null && loginRequest.Password != null)
            {
                var user = _context.Users.FirstOrDefault(x => x.Username == loginRequest.Username &&  x.Password == GetMD5(loginRequest.Password));
                if (user != null)
                {
                    var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Username",user.Username)
                    };
                    var userRoles = _context.UserRoles.Where(u => u.UserId == user.Id).ToList();
                    var roleIds = userRoles.Select(u => u.RoleId).ToList(); 
                    var roles = _context.Roles.Where(r => roleIds.Contains(r.Id)).ToList();
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim("Role", role.Name));
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: signIn
                    );
                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
                }
                else
                {
                    return "tài khoản không hợp lệ!";
                }
            }
            else
            {
                return "chưa nhập username và password!";
            }
        }

        public async Task<UserDto> RegisterWithFacebook(UserDto user)
        {
            var userOld = _context.Users.Where(x => x.Id == user.Id).FirstOrDefault();
            if (userOld == null)
            {
                // Convert timestamp to DateTime
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(user.NgaySinh!));

                // Get DateTime object
                DateTime dateTime = dateTimeOffset.DateTime;
                var newUser = new User
                {
                    Id = user.Id,
                    Email = user.Email,
                    Ten = user.Ten,
                    Username = Guid.NewGuid().ToString(),
                    Password = Guid.NewGuid().ToString(),
                    DiaChi = user.DiaChi,
                    GioiTinh = user.GioiTinh,
                    NgaySinh = dateTime,
                    SoDienThoai = user.SoDienThoai
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync(new CancellationToken());

                var newUserRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = 2
                };
                _context.UserRoles.Add(newUserRole);
                await _context.SaveChangesAsync(new CancellationToken());

                return new UserDto
                {
                    Id = userOld.Id,
                    Email = userOld.Email,
                    Ten = userOld.Ten,
                    DiaChi = userOld.DiaChi,
                    GioiTinh = userOld.GioiTinh,
                    NgaySinh = userOld.NgaySinh!.Value.ToString("dd/MM/yyyy"),
                    SoDienThoai = user.SoDienThoai
                };
            }
            else
            {
                var userr = new UserDto
                {
                    Id = userOld.Id,
                    Email = userOld.Email,
                    Ten = userOld.Ten,
                    DiaChi = userOld.DiaChi,
                    GioiTinh = userOld.GioiTinh,
                    NgaySinh = userOld.NgaySinh!.Value.ToString("dd/MM/yyyy"),
                    SoDienThoai = user.SoDienThoai
                };
               return userr;
            }
        }

        public async Task ChangedPassword(string userId, ChangedPasswordDto dataChange)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                {
                    throw new Exception("Không tìm thấy dữ liệu người dùng!");
                }

                if(user.Password != GetMD5(dataChange.OldPassword))
                {
                    throw new Exception("Mật khẩu không đúng!");
                }

                if (user.Password == GetMD5(dataChange.OldPassword))
                {
                    user.Password = GetMD5(dataChange.NewPassword);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAccount(string userId, ChangedPasswordDto dataChange)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                {
                    throw new Exception("Không tìm thấy dữ liệu người dùng!");
                }
                if (user.Password != GetMD5(dataChange.OldPassword))
                {
                    throw new Exception("Mật khẩu không đúng!");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                var thumuc = _context.ThuMucs.Where(x => x.UserId == userId).AsNoTracking();
                _context.ThuMucs.RemoveRange(thumuc);
                await _context.SaveChangesAsync();

                var hocphans = _context.HocPhans.Where(x=> x.UserId == userId).AsNoTracking();
                foreach (var item in hocphans)
                {
                    var thehoc = _context.TheHocs.Where(x => x.HocPhanId == item.Id).AsNoTracking();
                    _context.TheHocs.RemoveRange(thehoc);
                    await _context.SaveChangesAsync();
                }
                _context.HocPhans.RemoveRange(hocphans);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
