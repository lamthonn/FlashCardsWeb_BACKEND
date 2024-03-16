using backend_v3.Dto;

namespace backend_v3.Interfaces
{
    public interface IFacebookAuthenticator
    {
        public Task<bool> ValidateAccessTokenAsync(FacebookRequest request);
    }
}
