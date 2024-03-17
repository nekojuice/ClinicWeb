namespace ClinicWeb.Models.MBSatisfactionViewModel
{
    public class MedicalRecordsVM
    {
        public int MrId { get; set; }
        public int? PatientSatisfaction { get; set; }

        public int? DocSatisfaction { get; set; }

        public int? ClinicSatisfaction { get; set; }

        public int? SysSatisfaction { get; set; }
    }
}
