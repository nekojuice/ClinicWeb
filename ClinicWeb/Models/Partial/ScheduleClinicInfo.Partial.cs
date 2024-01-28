using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicWeb.Models
{
    [MetadataType(typeof(ScheduleClinicInfoMetadata))]
    public partial class ScheduleClinicInfo
    {
    }

    internal class ScheduleClinicInfoMetadata
    {

        public int ClinicInfoId { get; set; }

        public string Date { get; set; }

        public int? RegistrationLimit { get; set; }


        
        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<ApptClinicList> ApptClinicList { get; set; } = new List<ApptClinicList>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<CasesMedicalRecords> CasesMedicalRecords { get; set; } = new List<CasesMedicalRecords>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual RoomList ClincRoom { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ScheduleClinicTime ClinicTime { get; set; }

        
        public virtual MemberEmployeeList Doctor { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<ScheduleNurseSchedule> ScheduleNurseSchedule { get; set; } = new List<ScheduleNurseSchedule>();
    }
}
