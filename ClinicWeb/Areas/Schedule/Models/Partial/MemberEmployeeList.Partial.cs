using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Areas.Schedule.Models.Partial
{
    [MetadataType(typeof(MemberEmployeeListMetaData))]
    public partial class MemberEmployeeList
    {
    }

    internal class MemberEmployeeListMetaData
    {
        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<ScheduleClinicInfo> ScheduleClinicInfo { get; set; } = new List<ScheduleClinicInfo>();
    }
}
