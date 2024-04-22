using backend_v3.Context;
using backend_v3.Controllers.common;
using backend_v3.Dto;
using backend_v3.Dto.Common;
using backend_v3.Interfaces;
using backend_v3.Models;
using backend_v3.Seriloger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Ilogger = Serilog.ILogger;

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;
        private readonly IMailService _mail_sevice;
        private readonly Ilogger _logger;
        private readonly LoggingCommon _loggingCommon;
        public UserController(IUserService service, Ilogger logger, IWebHostEnvironment environment, AppDbContext context, IMailService mail_sevice)
        {
            _service = service;
            _environment = environment;
            _context = context;
            _mail_sevice = mail_sevice;
            _logger = logger;
            _loggingCommon = new LoggingCommon(_logger, _context);
        }

        [HttpGet]
        [Authorize]
        public Task<UserDto> GetUserbyId (string id)
        {
            try
            {
                var res = _service.GetUserById(id);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public Task<List<User>> GetAllUser()
        {
            try
            {
                var res = _service.GetAllUser();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public Task<PaginatedList<User>> GetPaginUser([FromQuery]UserRequest request)
        {
            try
            {
                var res = _service.GetPaginUser(request);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public Task<List<Role>> GetAllRole()
        {
            try
            {
                var res = _service.GetAllRole();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<UserDto> AddUser(UserDto user)
        {
            try
            {
                var res = await _service.AddUser(user);
                 _loggingCommon.AddLoggingInformation(
                    $"Thêm người dùng {user.Username}",
                    user.UserId,
                    LoggingType.NHAT_KY_THAO_TAC_QUAN_TRI
                );
                return res;
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                    $"Lỗi thêm người dùng: {ex.Message}",
                    user.UserId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                throw new Exception(ex.Message);
            }
        }
        [HttpPut]
        public async Task<bool> EditInforUser(string id, UserDto userData)
        {
            try
            {
                await _service.EditInforUser(id, userData);
                _loggingCommon.AddLoggingInformation(
                    $"Thay đổi thông tin cá nhân",
                    id,
                    LoggingType.NHAT_KY_THAO_TAC_NGUOI_DUNG
                );
                return true;
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                    $"Lỗi thay dổi thông tin cá nhân",
                    userData.UserId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                return false;
            }
        }

        [HttpPut]
        public async Task<bool> EditInforUser_Admin(string id, UserDto userData)
        {
            try
            {
                await _service.EditInforUser_Admin(id, userData);
                _loggingCommon.AddLoggingInformation(
                    $"sửa thông tin người dùng {userData.Username}",
                    userData.UserId,
                    LoggingType.NHAT_KY_THAO_TAC_QUAN_TRI
                );
                return true;
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                    $"Lỗi sửa người dùng: {ex.Message}",
                    userData.UserId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                return false;
            }
        }

        [HttpDelete]
        public async Task<bool> DeleteUser(string id, string? userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            try
            {
                await _service.DeleteUser_Admin(id);
                _loggingCommon.AddLoggingInformation(
                    $"Xóa thông tin người dùng {user.Username}",
                    userId,
                    LoggingType.NHAT_KY_THAO_TAC_QUAN_TRI
                );
                return true;
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                    $"Lỗi xóa người dùng: {ex.Message}",
                    userId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                return false;
            }
        }

        [HttpPost]
        public Task SendMail(MailData mailData)
        {
            return _mail_sevice.SendMail(mailData);
        }

        [HttpPut]
        public async Task<bool> UpdateAvatar([FromBody] UpdateAvt data )
        {
            try
            {
                await _service.UpdateAvatar(data.id, data.path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPut]
        public async Task<bool> ResetPassWord(string id,string? userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            try
            {
                await _service.ResetPassWord(id);
                _loggingCommon.AddLoggingInformation(
                    $"Thay đổi lại mật khẩu cho người dùng {user.Username}",
                    userId,
                    LoggingType.NHAT_KY_THAO_TAC_QUAN_TRI
                );
                return true;
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                    $"Lỗi đổi lại mật khẩu cho người dùng: {ex.Message}",
                    userId,
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                return false;
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetFile( string id)
        {
            var path = _context.Users.FirstOrDefault(x => x.Id == id)!.AnhDaiDien;
            try
            {
                var pathFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/", path);

                if (System.IO.File.Exists(pathFile))
                {
                    FileInfo fileInfo = new FileInfo(pathFile);
                    var realFilePath = fileInfo.FullName;
                    //chong doc ra ngoai thu muc
                    if (realFilePath.IndexOf("avatar") < 0) return NotFound();
                    //var fileName = System.IO.Path.GetFileName(pathFile);
                    string fileName = fileInfo.Name;
                    //string fileExtension = fileInfo.Extension;
                    new FileExtensionContentTypeProvider()
                                    .TryGetContentType(fileName, out string contentType);
                    var fileContent = await System.IO.File.ReadAllBytesAsync(pathFile);

                    return File(fileContent, contentType, fileName);
                }

                return NotFound(); // returns a NotFoundResult with Status404NotFound response.
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
                return Problem(detail: ioExp.Message);
            }
        }

        [HttpPost]
        public async Task<string> UploadFileAsync(FileUploadBytes fileUpload)
        {
            if (fileUpload == null)
            {
                throw new ArgumentNullException(nameof(fileUpload));
            }

            if (string.IsNullOrWhiteSpace(fileUpload.fileName))
            {
                throw new ArgumentException("File name is missing or empty.", nameof(fileUpload));
            }

            if (string.IsNullOrWhiteSpace(fileUpload.contentType))
            {
                throw new ArgumentException("File content type is missing or empty.", nameof(fileUpload));
            }

            try
            {
                // Construct the path to the avatar directory under wwwroot
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "avatar");

                // Check if the avatar directory exists, create it if not
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Giải mã dữ liệu base64 thành byte array
                byte[] imageBytes = Convert.FromBase64String(fileUpload.contentBase64);

                // Lưu ảnh vào thư mục wwwroot/avatar
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileUpload.fileName;
                string filePath = Path.Combine(_environment.WebRootPath, "avatar", uniqueFileName);
                
                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                //await File.WriteAllBytesAsync(filePath, fileUpload.content);

                // Tạo đường dẫn ảnh lưu trữ
                string imageUrl = $"avatar/{uniqueFileName}";

                // Trả về đường dẫn ảnh lưu trữ hoặc base64 string (tùy chọn)
                return imageUrl;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while uploading file.", ex);
            }
        }
        
    }
}
