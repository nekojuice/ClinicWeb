using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Controllers.ScheduleAPI
{
    public class WeekScheduleController : Controller
    {
        private readonly ClinicSysContext _context;
        public WeekScheduleController(ClinicSysContext context)
        {
            _context = context;

        }
        [Route("{area}/{controller}/{action}/{departmentName}")]
        [HttpGet]
        public IActionResult Get_WeekSchedule(string departmentName)
        {
            //撈取資料到物件
            var result = _context.ScheduleClinicSchedule
                .Where(x => x.Doctor.Department == departmentName)
                .GroupBy(x => x.Week)
                .Select(x =>
                    new
                    {
                        Week = x.Key,
                        ClinicShifts = x.GroupBy(x => x.Time.ClinicShifts).Select(x =>
                        new
                        {
                            ClinicShift = x.Key,
                            Details = x.Select(x => new
                            {
                                doctor = x.Doctor.Name,
                                room = x.Room.Name
                            })
                        })
                    }
                );
            //將物件重組成動態json key
            var rebuildFormat = result.ToDictionary(
                w => w.Week, w => (object)w.ClinicShifts.ToDictionary(
                    w => w.ClinicShift, w => (object)w.Details
                ));

            return Json(rebuildFormat);
        }
        //用法: fetch("/Schedule/Api/Get_WeekSchedule/小兒科")


        //醫師評價平均值
        [HttpPost]
        //[Route("{controller}/{action}/{id}")]
        public JsonResult GS(string id) //GET
        {
            //int Daverage = 0;
            //int Caverage = 0;

            double Daverage = 0;
            double Caverage = 0;

            var a = _context.CasesMedicalRecords
                .Include(x => x.ClinicList.Clinic.Doctor)
                .Where(x => x.ClinicList.Clinic.Doctor.EmpId == Convert.ToInt32(id))
                .Select(x => new
                {
                    sat = x.DocSatisfaction,
                    at = x.PatientSatisfaction
                });


            int dc = a.Count();
            int cc = a.Count();
            foreach (var item in a)
            {
                if (item.sat.HasValue)
                { Daverage += item.sat.Value; }
                else
                { dc--; }
                if (item.at.HasValue)
                { Caverage += item.at.Value; }
                else
                { cc--; }


            }
            Daverage = Math.Round(Daverage / dc,1);
            Caverage = Math.Round(Caverage / cc,1);
            return Json(new { DocSatisfaction = Daverage.ToString("0.0"),
                PatientSatisfaction = Caverage.ToString("0.0")
            });
        }
    }
}
    
