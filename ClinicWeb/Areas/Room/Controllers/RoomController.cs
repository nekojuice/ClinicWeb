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

        // GET: Room/RoomReservation
        //public async Task<IActionResult> Index(int? nurseId, int? doctorId, DateTime? startDate, DateTime? endDate)
        //{
        //    var roomReservationsQuery = _context.AppointmentRoomSchedule
        //        .Include(a => a.Emp)
        //        .Include(a => a.Member)
        //        .Include(a => a.Room)
        //        .AsQueryable();
            //var appointments = _context.AppointmentRoomSchedule
            //  .Include(a => a.Room)
            //  .Include(a => a.Member)
            //  .ToList();
            //    var roomReservationsQuery = _context.AppointmentRoomSchedule
            //        .Select(x => new ShowAppointmentRoomSchedule
            //        {
            //            AppointmentId = x.AppointmentId,
            //            DoctorId = x.DoctorId,
            //            EmpId = x.EmpId,
            //            EndDate = x.EndDate,
            //            StartDate = x.StartDate,
            //            MemberId = x.MemberId,
            //            NurseId = x.NurseId,
            //            RoomId = x.RoomId,
            //            MemberName = x.Member.Name,
            //            RoomName = x.Room.Name
            //        });


            //    //if (nurseId != null)
            //    //{
            //    //    roomReservationsQuery = roomReservationsQuery.Where(a => a.EmpId == nurseId);
            //    //}

            //    //if (doctorId != null)
            //    //{
            //    //    roomReservationsQuery = roomReservationsQuery.Where(a => a.EmpId == doctorId);
            //    //}

            //    //if (startDate != null)
            //    //{
            //    //    string a = startDate?.ToString("yyyy/MM/dd");
            //    //    roomReservationsQuery = roomReservationsQuery.Where(a => a.StartDate.CompareTo(a) >= 0);
            //    //}

            //    //if (endDate != null)
            //    //{ 
            //    //    string b = endDate?.ToString("yyyy/MM/dd");
            //    //    roomReservationsQuery = roomReservationsQuery.Where(a => a.EndDate.CompareTo(b) <= 0);
            //    //}

            //    //var roomReservations = await roomReservationsQuery.ToListAsync();
            //    //var viewModel = new AppointmentRoomSchedule
            //    //{

            //    //    NurseId = nurseId,
            //    //    DoctorId = doctorId,
            //    //    StartDate = startDate?.ToString("yyyy/MM/dd"),
            //    //    EndDate = endDate?.ToString("yyyy/MM/dd")
        //    //};
        //    return View(roomReservationsQuery);
        //}

        // GET: Room/RoomReservation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Room/RoomReservation/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(RoomReservationViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(viewModel.NewReservation);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(viewModel);
        //}
        public IActionResult Index(string RoomId, string memberId, string nurseId, string doctorId,string DoctorName, string NurseName, DateTime? startDate, DateTime? endDate)
        {
            var employees = _context.MemberEmployeeList.ToList();
            
            var a = _context.AppointmentRoomSchedule

            .Include(a => a.Emp)
            .Include(a => a.Member)
            .Include(a => a.Room) .ThenInclude(r => r.Type)
         
                   
           
             .ToList();

            List<ShowAppointmentRoomSchedule> list = new List<ShowAppointmentRoomSchedule>();






            foreach (var appointment in a)
            {

                ShowAppointmentRoomSchedule ap = new ShowAppointmentRoomSchedule();
                ap.AppointmentId = appointment.AppointmentId;
                ap.RoomId = appointment.RoomId;
                ap.RoomName = appointment.Room.Type.Name;
                ap.MemberId = appointment.MemberId;
                ap.MemberName = appointment.Member.Name;
                ap.StartDate = appointment.StartDate;
                ap.EndDate = appointment.EndDate;
                ap.DoctorId = appointment.DoctorId;

                ap.NurseId = appointment.NurseId;

                foreach (var employee in employees)
                {


                    if (employee.EmpType == "醫生")
                    {
                        ap.DoctorId =appointment.EmpId;
                        ap.DoctorName = appointment.Emp.Name;
                    }

                    if (employee.EmpType == "護士")
                    {
                        ap.NurseId=appointment.EmpId;
                        ap.NurseName = appointment.Emp.Name;
                    }
                     list.Add(ap);
                }
            }

           return View(list);       
          }



            
    
            
    }
 }

