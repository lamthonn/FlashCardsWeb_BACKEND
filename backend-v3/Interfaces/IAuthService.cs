using backend_v3.Dto;
using backend_v3.Models;

namespace backend_v3.Interfaces
{
    public interface IAuthService
    {
        AddUserRole Register (AddUserRole user);
        string Login(LoginRequest loginRequest);
        Role AddRole (Role role);
        //bool AssignRoleToUser(AddUserRole obj);
    }
}
