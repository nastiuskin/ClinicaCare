using Domain.Intefraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MedicalProcedures
{
    public record MedicalProcedureId(Guid Value) : ITypedId;
}
