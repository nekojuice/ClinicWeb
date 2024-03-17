using ClinicWeb.Areas.Appointment.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class FArrivalController : Controller
    {
        ClinicSysContext _context;
        public FArrivalController(ClinicSysContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{controller}/{action}/{year}/{month}/{day}/{nationalId}/{ip}")]
        public IActionResult Remote_CardInsert(string year, string month, string day, string nationalId, string ip) 
        {
            string dateString = $"{year}/{month}/{day}";
            Console.WriteLine($"{dateString}, {nationalId}, {ip}");

            //送訊息到hub
            var target = Json(_context.ApptClinicList.Where(x => x.Clinic.Date == dateString && x.Member.NationalId == nationalId));


            return Ok();
        }


    }
}
