using Application.UserAccountManagement.UserDtos;
using Domain.Users.Doctors;
using System.ComponentModel.DataAnnotations;

namespace Application.UserAccountManagement.Doctors.DTO
{
    public class DoctorFormDto : UserFormDto
    {
        [Required(ErrorMessage = "Specialization is required.")]
        public SpecializationType Specialization {  get; set; }
        public string Biography { get; set; }

        [Required(ErrorMessage = "Cabinet number is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cabinet number must be greater than zero.")]

        public int CabinetNumber { get; set; }

        [Required(ErrorMessage = "Working hours start is required.")]
        public string WorkingHoursStart { get; set; }

        [Required(ErrorMessage = "Working hours end is required.")]
        public string WorkingHoursEnd {  get; set; }
    }
}
