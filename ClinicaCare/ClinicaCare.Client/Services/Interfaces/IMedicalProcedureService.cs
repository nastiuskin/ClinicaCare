using Application.AppointmentManagement.DTO;
using Application.Helpers.PaginationStuff;
using Application.MedicalProcedureManagement.DTO;
using Application.UserAccountManagement.UserDtos;
using ClinicaCare.Client.Services.Pagination;
using Domain.MedicalProcedures;

namespace ClinicaCare.Client.Services.Interfaces
{
    public interface IMedicalProcedureService
    {
        public Task<PagingResponse<MedicalProcedureInfoDto>> GetAllMedicalProceduresAsync(MedicalProcedureParameters parameters);
        public Task<MedicalProcedureInfoWithDoctorsDto> GetMedicalProcedureAsync(Guid id);
        public Task<bool> DeleteMedicalProcedure(Guid id);
        public Task<bool> CreateMedicalProcedureAsync(MedicalProcedureFormDto medicalProcedureFormDto);
        public Task<bool> UpdateMedicalProcedureAsync(Guid id, MedicalProcedureUpdateDto medicalProcedureUpdateDto);
    }
}
