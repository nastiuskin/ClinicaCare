namespace Domain.Entities.Patient
{
    public class MedicalRecord
    {
        //must be created after completing appontment by doctor
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
