using Azure.Core;
using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using backend_v3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IFacebookAuthenticator _facebookAuthenticator;
        public AuthController(IAuthService auth, IFacebookAuthenticator facebookAuthenticator)
        {
            _auth = auth;
            _facebookAuthenticator = facebookAuthenticator;
        }

        [HttpPost]
        public string Login( LoginRequest obj)
        {
            var token = _auth.Login(obj);
            return token;
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

    }
}
