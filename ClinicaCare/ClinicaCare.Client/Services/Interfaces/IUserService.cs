using Application.UserAccountManagement.UserDtos;

namespace ClinicaCare.Client.Services.Interfaces
{
    public interface IUserService
    {
        public Task<(UserViewDto User, string ErrorMessage)> GetProfile();
        public Task<(bool Success, string[] Errors)> UpdateAsync(UserViewDto userViewDto);
    }
}
