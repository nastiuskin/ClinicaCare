using Application.UserAccountManagement.UserDtos;

namespace ClinicaCare.Client.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<(bool Success, string[] Errors)> LoginAsync(UserLoginDto userLoginDto);
        public Task<(bool Success, string[] Errors)> RegisterAsync(UserFormDto userFormDto);
        public Task<(bool Success, string[] Errors)>  LogoutAsync();
        public Task<(bool Success, string? Token, string? ErrorMessage)> RefreshTokenAsync();
    }
}
