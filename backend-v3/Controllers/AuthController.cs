using backend_v3.Dto;
using backend_v3.Interfaces;
using backend_v3.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend_v3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost]
        public string Login([FromBody] LoginRequest obj)
        {
            var token = _auth.Login(obj);
            return token;
        }

        //[HttpPost]
        //public bool AssignRoleToUser([FromBody] AddUserRole UserRole)
        //{
        //    var addUserRole = _auth.AssignRoleToUser(UserRole);
        //    return addUserRole;
        //}

        [HttpPost]
        public AddUserRole Register([FromBody] AddUserRole user)
        {
            var addedUser = _auth.Register(user);
            return addedUser;
        }

        [HttpPost]
        public Role AddRole([FromBody] Role role)
        {
            var addedRole = _auth.AddRole(role);
            return addedRole;
        }

    }
}
