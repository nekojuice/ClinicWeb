using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Areas.Room.Models
{
    public class ShowAppointmentRoomSchedule
    {
        public int AppointmentId { get; set; }

        public int EmpId { get; set; }

        public int? RoomId { get; set; }        
        public string? RoomName { get; set; }
        
        public int? MemberId { get; set; }

        public string? MemberName { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }

        public int? DoctorId { get; set; }
        public string? DoctorName { get; set; }

        public int? NurseId { get; set; }
        public string? NurseName { get; set; }

        [ForeignKey("EmpId")]
        [InverseProperty("AppointmentRoomSchedule")]
        public virtual MemberEmployeeList? Emp { get; set; }

        [ForeignKey("MemberId")]
        [InverseProperty("AppointmentRoomSchedule")]
        public virtual MemberMemberList? Member { get; set; }

        [ForeignKey("RoomId")]
        [InverseProperty("AppointmentRoomSchedule")]
        public virtual RoomList? Room { get; set; }
    }
}
