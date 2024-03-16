using backend_v3.Dto;
using backend_v3.Models;

namespace backend_v3.Interfaces
{
    public interface IAuthService
    {
        public AddUserRole Register (AddUserRole user);
        public Task<UserDto> RegisterWithFacebook (UserDto user);
        public string Login(LoginRequest loginRequest);
        public Role AddRole ( Role role);
        //bool AssignRoleToUser(AddUserRole obj);
    }
}
