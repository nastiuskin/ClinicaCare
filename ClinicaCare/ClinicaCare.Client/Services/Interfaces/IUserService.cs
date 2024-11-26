﻿using Application.Helpers.PaginationStuff;
using Application.UserAccountManagement.Doctors.DTO;
using Application.UserAccountManagement.UserDtos;
using ClinicaCare.Client.Services.Pagination;
using Domain.Helpers.PaginationStuff;

namespace ClinicaCare.Client.Services.Interfaces
{
    public interface IUserService
    {
        public Task<(UserViewDto User, string ErrorMessage)> GetProfile();
        public Task<(bool Success, string[] Errors)> UpdateAsync(UserViewDto userViewDto);
        public Task<PagingResponse<DoctorPartialInfoDto>> GetPagiantedDoctorsAsync(UserParameters parameters);
        public Task<(bool Success,List<DoctorPartialInfoDto>)> GetAllDoctorsAsync();
        public Task<bool> DeleteUser(Guid id);
        public Task<bool> CreateDoctorAsync(DoctorFormDto doctorFormDto);

    }
}
