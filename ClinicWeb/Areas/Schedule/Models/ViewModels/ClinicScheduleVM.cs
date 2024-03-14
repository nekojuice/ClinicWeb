using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Schedule.Models.ViewModels
{
    public class ClinicScheduleVM : Controller
    {
        public int ScheduleId { get; set; }
        public int DoctorId { get; set; }
        public int Week { get; set; }
        public int TimeId { get; set; }
        public int RoomId { get; set; }
    }
}
