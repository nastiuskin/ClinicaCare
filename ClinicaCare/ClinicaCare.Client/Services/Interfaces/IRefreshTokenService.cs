namespace ClinicaCare.Client.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        public Task<string?> RefreshTokenAsync();
    }
}
