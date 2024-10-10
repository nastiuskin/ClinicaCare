using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Aggregates.Appointment.Patient
{
    public class MedicalRecord
    {
        public Patient Patient { get; private set; }
        public DateTime DateOfRecord { get; private set; }
        public string Diagnosis { get; private set; }
        public string Treatment { get; private set; }
        public string PrescribedMedication { get; private set; }

        private MedicalRecord(Patient patient, DateTime dateOfRecord, string diagnosis, string treatment, string prescribedMedication)
        {
            Patient = patient;
            DateOfRecord = dateOfRecord;
            Diagnosis = diagnosis;
            Treatment = treatment;
            PrescribedMedication = prescribedMedication;
        }

        public static MedicalRecord Create(Patient patient, DateTime dateOfRecord, string diagnosis, string treatment, string prescribedMedication)
        {
            return new MedicalRecord(patient, dateOfRecord, diagnosis, treatment, prescribedMedication);
        }

    }
}
