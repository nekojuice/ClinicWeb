using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Room.Models;
using ClinicWeb.Areas.Room.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(int? nurseId, int? doctorId, DateTime? startDate, DateTime? endDate)
        {
            var roomReservationsQuery = _context.AppointmentRoomSchedule
                .Include(a => a.Emp)
                .Include(a => a.Member)
                .Include(a => a.Room)
                .AsQueryable();

            //if (nurseId != null)
            //{
            //    roomReservationsQuery = roomReservationsQuery.Where(a => a.EmpId == nurseId);
            //}

            //if (doctorId != null)
            //{
            //    roomReservationsQuery = roomReservationsQuery.Where(a => a.EmpId == doctorId);
            //}

            //if (startDate != null)
            //{
            //    string a = startDate?.ToString("yyyy/MM/dd");
            //    roomReservationsQuery = roomReservationsQuery.Where(a => a.StartDate.CompareTo(a) >= 0);
            //}

            //if (endDate != null)
            //{ 
            //    string b = endDate?.ToString("yyyy/MM/dd");
            //    roomReservationsQuery = roomReservationsQuery.Where(a => a.EndDate.CompareTo(b) <= 0);
            //}

            //var roomReservations = await roomReservationsQuery.ToListAsync();
            //var viewModel = new AppointmentRoomSchedule
            //{

            //    NurseId = nurseId,
            //    DoctorId = doctorId,
            //    StartDate = startDate?.ToString("yyyy/MM/dd"),
            //    EndDate = endDate?.ToString("yyyy/MM/dd")
            //};
            return View(roomReservationsQuery);
        }

        // GET: Room/RoomReservation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Room/RoomReservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomReservationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.NewReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // Other actions: Edit, Delete, Details, etc.
    }
}
