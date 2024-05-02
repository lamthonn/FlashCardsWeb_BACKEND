using Azure.Core;
using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using backend_v3.Seriloger;
using backend_v3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Ilogger = Serilog.ILogger;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IFacebookAuthenticator _facebookAuthenticator;
        private readonly AppDbContext _context;
        private readonly Ilogger _logger;
        private readonly LoggingCommon _loggingCommon;
        public AuthController(IAuthService auth, IFacebookAuthenticator facebookAuthenticator, Ilogger logger, AppDbContext context)
        {
            _auth = auth;
            _facebookAuthenticator = facebookAuthenticator;
            _context = context;
            _logger = logger;
            _loggingCommon = new LoggingCommon(_logger, _context);
        }

        [HttpPost]
        public string Login( LoginRequest obj)
        {
            try
            {
                var token = _auth.Login(obj);
                var user = _context.Users.FirstOrDefault(x => x.Username == obj.Username);
                _loggingCommon.AddLoggingInformation(
                    "Đăng nhập web application VocaLearn",
                    user.Id,
                    LoggingType.NHAT_KY_TRUY_CAP_HE_THONG
                );
                return token;
            }
            catch (Exception ex)
            {
                _loggingCommon.AddLoggingError(
                    $"Lỗi đăng nhập web application VocaLearn: {ex.Message}",
                    "",
                    LoggingType.NHAT_KY_LOI_PHAT_SINH
                );
                throw new Exception(ex.Message);
            }
        }

        //[HttpPost]
        //public bool AssignRoleToUser( AddUserRole UserRole)
        //{
        //    var addUserRole = _auth.AssignRoleToUser(UserRole);
        //    return addUserRole;
        //}

        [HttpPost]
        public AddUserRole Register( AddUserRole user)
        {
            var addedUser = _auth.Register(user);
            return addedUser;
        }

        [HttpPost]
        public Task<UserDto> RegisterWithFacebook( UserDto user)
        {
            try
            {
                var addedUserfb = _auth.RegisterWithFacebook(user);
                return addedUserfb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        [HttpPost]
        public Role AddRole( Role role)
        {
            var addedRole = _auth.AddRole(role);
            return addedRole;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateFacebookToken([FromBody] FacebookRequest request )
        {
            if (string.IsNullOrEmpty(request.AccessToken))
            {
                return BadRequest("Access token is required.");
            }

            try
            {
                bool isValid = await _facebookAuthenticator.ValidateAccessTokenAsync(request);
                return Ok(new { IsValid = isValid });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error validating Facebook token: {ex.Message}");
            }
        }

        [HttpPut]
        public Task ChangePassword(string userId, ChangedPasswordDto data)
        {
            try
            {
                var addedUserfb = _auth.ChangedPassword(userId,data);
                return addedUserfb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        
        [HttpDelete]
        public Task DeleteAccount(string userId, ChangedPasswordDto data)
        {
            try
            {
                var deleteAcc = _auth.DeleteAccount(userId,data);
                return deleteAcc;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
