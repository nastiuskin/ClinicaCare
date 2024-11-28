namespace ClinicaCare.Client.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        public Task<bool> RefreshTokenAsync();
    }
}
