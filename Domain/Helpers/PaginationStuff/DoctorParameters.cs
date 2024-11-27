using Domain.MedicalProcedures;
using Domain.Users.Doctors;

namespace Domain.Helpers.PaginationStuff
{
    public class DoctorParameters : UserParameters
    {
        public Guid? MedicalProcedureId { get; set; }

    }
}
