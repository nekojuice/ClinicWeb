using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicWeb.Models
{
    [MetadataType(typeof(MemberEmployeeListMetadata))]
    public partial class MemberEmployeeList
    {
    }

    internal class MemberEmployeeListMetadata 
    {
        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<AppointmentRoomSchedule> AppointmentRoomScheduleDoctor { get; set; } = new List<AppointmentRoomSchedule>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<AppointmentRoomSchedule> AppointmentRoomScheduleNurse { get; set; } = new List<AppointmentRoomSchedule>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<AttendanceTAttendance> AttendanceTAttendance { get; set; } = new List<AttendanceTAttendance>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<AttendanceTExpenseRequests> AttendanceTExpenseRequests { get; set; } = new List<AttendanceTExpenseRequests>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<AttendanceTLeave> AttendanceTLeave { get; set; } = new List<AttendanceTLeave>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<ScheduleClinicInfo> ScheduleClinicInfo { get; set; } = new List<ScheduleClinicInfo>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<ScheduleClinicSchedule> ScheduleClinicSchedule { get; set; } = new List<ScheduleClinicSchedule>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<ScheduleNurseSchedule> ScheduleNurseSchedule { get; set; } = new List<ScheduleNurseSchedule>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<SchedulePharmacistSchedule> SchedulePharmacistSchedule { get; set; } = new List<SchedulePharmacistSchedule>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<ScheduleStaffSchedule> ScheduleStaffSchedule { get; set; } = new List<ScheduleStaffSchedule>();

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<ScheduleWardNurseSchedule> ScheduleWardNurseSchedule { get; set; } = new List<ScheduleWardNurseSchedule>();
    }
}
