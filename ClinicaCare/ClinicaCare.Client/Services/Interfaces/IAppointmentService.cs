using Application.AppointmentManagement.DTO;
using ClinicaCare.Client.Services.Pagination;
using Domain.Helpers.PaginationStuff;

namespace ClinicaCare.Client.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task<PagingResponse<AppointmentInfoDto>> GetAllAppontmentsAsync(AppointmentParameters parameters);

    }
}
