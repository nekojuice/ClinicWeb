using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Room.Models;
using Microsoft.EntityFrameworkCore;
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

        // 显示创建表单
        public IActionResult Create()
        {
            return View();
        }

        // 处理创建操作
        [HttpPost]
        public async Task<IActionResult> Create(ShowAppointmentRoomSchedule model)
        {
            if (ModelState.IsValid)
            {
                // 添加到数据库
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 显示编辑表单
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.AppointmentRoomSchedule.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // 处理编辑操作
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ShowAppointmentRoomSchedule model)
        {
            if (id != model.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 更新数据库
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(model.AppointmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 显示删除确认页面
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.AppointmentRoomSchedule
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // 处理删除操作
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.AppointmentRoomSchedule.FindAsync(id);
            _context.AppointmentRoomSchedule.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 显示预约列表
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

        private bool AppointmentExists(int id)
        {
            return _context.AppointmentRoomSchedule.Any(e => e.AppointmentId == id);
        }
    }
}
