using System;
using System.Collections.Generic;
using ClinicWeb.Areas.Room.Models;

namespace ClinicWeb.Areas.Room.ViewModels
{
    public class RoomReservationViewModel
    {
        public IEnumerable<AppointmentRoomSchedule> RoomReservations { get; set; }
        public int? NurseId { get; set; }
        public int? DoctorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AppointmentRoomSchedule NewReservation { get; set; }
    }
}
