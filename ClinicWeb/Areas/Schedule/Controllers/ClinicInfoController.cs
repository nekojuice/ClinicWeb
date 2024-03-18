using ClinicWeb.Areas.Schedule.Models;
using ClinicWeb.Areas.Schedule.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Schedule.Controllers
{
    [Area("Schedule")]//路由設定123
    public class ClinicInfoController : Controller
    {
        private readonly ClinicSysContext _context;
        public ClinicInfoController(ClinicSysContext context)
        {
            _context = context;

        }
        public IActionResult ShowWeekSchedule() //匯出周表
        {

            var WeekSchedule = _context.ScheduleClinicSchedule
                .Select(x => new
                {
                    id = x.ScheduleId,
                    Week = x.Week,
                    醫師 = x.Doctor.Name,
                    星期 = GetDayOfWeek(x.Week),
                    時段 = x.Time.ClinicShifts,
                    科別 = x.Doctor.Department,
                    診間 = x.Room.Name

                });

            return Json(WeekSchedule);
        }

        public IActionResult ShowRoom()
        {
            var Week = _context.RoomList
                .Where(m => m.TypeId == 3)
                .Select(x => new
                {
                    x.RoomId,
                    x.Name
                });
            return Json(Week);
        }

        //新增門診
        [Route("{area}/{controller}/{action}/{drid}/{week}/{shiftid}/{roomid}")]
        public IActionResult AddClinic(string drid, string week, string shiftid, string roomid)
        {
            // 檢查是否已存在相同門診
            var existingClinic = _context.ScheduleClinicSchedule
                .Any(x => x.DoctorId == Convert.ToInt32(drid) &&
                x.Week == Convert.ToInt32(week) &&
                x.TimeId == Convert.ToInt32(shiftid) &&
                x.RoomId == Convert.ToInt32(roomid));

            if (existingClinic == false)
            {// 如果不存在，則創建新的門診
                var newClinic = new ScheduleClinicSchedule
                {
                    DoctorId = Convert.ToInt32(drid),
                    Week = Convert.ToInt32(week),
                    TimeId = Convert.ToInt32(shiftid),
                    RoomId = Convert.ToInt32(roomid)
                };

                // 將新的門存進資料庫
                _context.ScheduleClinicSchedule.Add(newClinic);
                _context.SaveChanges();

                return Ok("門診已成功添加");

            }
            else
            {

                return BadRequest("門診已存在");
            }



        }

        private static string GetDayOfWeek(int week)
        {
            switch (week)
            {
                case 0:
                    return "星期日";
                case 1:
                    return "星期一";
                case 2:
                    return "星期二";
                case 3:
                    return "星期三";
                case 4:
                    return "星期四";
                case 5:
                    return "星期五";
                case 6:
                    return "星期六";
                default:
                    return "未知";
            }
        }

        [HttpPost]

        public IActionResult DeleteClinic([FromBody] ClinicScheduleVM model)
        {

            var clinicToDelete = _context.ScheduleClinicSchedule.FirstOrDefault(c => c.ScheduleId == model.ScheduleId);
            if (clinicToDelete != null)
            {
                _context.ScheduleClinicSchedule.Remove(clinicToDelete);
                _context.SaveChanges();
                return Ok("門診刪除成功");
            }
            else
            {
                return NotFound("未找到要刪除的門診");
            }


        }

    }
}
