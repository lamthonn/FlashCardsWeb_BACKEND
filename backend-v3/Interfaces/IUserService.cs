using backend_v3.Dto;
using backend_v3.Models;

namespace backend_v3.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto> GetUserById(string id);
    }
}
