using Application.AppointmentManagement.DTO;
using ClinicaCare.Client.Services.Pagination;
using Domain.Helpers.PaginationStuff;

namespace ClinicaCare.Client.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task<PagingResponse<AppointmentInfoDto>> GetAllAppontmentsAsync(AppointmentParameters parameters);
        public Task<List<TimeSlotDto>> GetAvailableTimeSlots(Guid doctorId, Guid medicalProcedureId, string date);
        public Task<bool> CreateAppointment(AppointmentFormDto appointmentFormDto);
        public Task<string> AddFeedback(Guid appointmentId, string feedback);

        //public Task<bool> EditAppointmentStatus(Guid appointmentId, string status);
    }
}
