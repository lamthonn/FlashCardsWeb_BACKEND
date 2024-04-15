using backend_v3.Dto;
using backend_v3.Models;

namespace backend_v3.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto> GetUserById(string id);
        public Task<List<User>> GetAllUser();
        public Task EditInforUser(string id, UserDto userData);
        public Task EditInforUser_Admin(string id, UserDto userData);
        public Task DeleteUser_Admin(string id);
        public Task UpdateAvatar(string id, string path);
        public Task<PaginatedList<User>> GetPaginUser(UserRequest request);
        public Task<List<Role>> GetAllRole();
        public Task<UserDto> AddUser(UserDto user);
        public Task ResetPassWord(string id);
    }
}
