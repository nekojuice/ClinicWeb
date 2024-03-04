using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Room.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClinicWeb.Areas.Room.Controllers
{
    [Area("Room")]
    public class RoomReservationController : Controller
    {
        private readonly ClinicSysContext _context;

        public RoomReservationController(ClinicSysContext context)
        {
            _context = context;
        }


       
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View(_context.AppointmentRoomSchedule
                .Select(x => new ShowAppointmentRoomSchedule()
                {
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DoctorName = x.Doctor.Name,
                    NurseName = x.Nurse.Name,
                    MemberName = x.Member.Name,
                    RoomName = x.Room.Name,
                }));
        }
           
        }
    
}

           
          



            
    
            
    
 

