using Domain.MedicalProcedures;
using System.ComponentModel.DataAnnotations;
namespace Application.MedicalProcedureManagement.DTO
{
    public class MedicalProcedureFormDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Medical procedure type is required.")]
        public MedicalProcedureType Type { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [RegularExpression(@"^\d{1,2}:[0-5][0-9]$", ErrorMessage = "Duration must be in format hh:mm")]
        public string Duration { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public List<Guid> Doctors { get; set; } = new List<Guid>();
    }
}
