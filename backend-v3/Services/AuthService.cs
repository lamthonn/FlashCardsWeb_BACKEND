using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.IdentityModel.Tokens;
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
                Id = role.Id,
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
                var user = _context.Users.FirstOrDefault(x => x.Username == loginRequest.Username && x.Password == GetMD5(loginRequest.Password));
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
    }
}
