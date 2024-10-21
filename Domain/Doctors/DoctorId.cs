using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Doctors
{
    public record DoctorId(Guid Value) : UserId(Value);
}
