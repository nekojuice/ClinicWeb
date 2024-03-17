using ClinicWeb.Areas.Appointment.Models;
using ClinicWeb.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol;
using Org.BouncyCastle.Asn1.X509;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace ClinicWeb.Controllers
{
    public class FArrivalController : Controller
    {
        private ClinicSysContext _context;
        private IHubContext<ArrivalHub, IArrivalHub> _hub { get; set; }
        public FArrivalController(ClinicSysContext context, IHubContext<ArrivalHub, IArrivalHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{controller}/{action}/{year}/{month}/{day}/{nationalId}/{ip}")]
        public async Task<IActionResult> Remote_CardInsert(string year, string month, string day, string nationalId, string ip)
        {
            string dateString = $"{year}/{month}/{day}";
            Console.WriteLine($"{dateString}, {nationalId}, {ip}");

            //送ip和資料到hub
            var memInfo = _context.MemberMemberList.Where(x => x.NationalId == nationalId)
                .Select(x => new
                {
                    c_name = x.Name,
                    c_gender = (bool)x.Gender ? "男" : "女",
                    c_nationalid = x.NationalId,
                    c_birthday = ((DateTime)x.BirthDate).ToString("yyyy-MM-dd")
                })
                .FirstOrDefault();
            var target = _context.ApptClinicList.Where(x => x.Clinic.Date == dateString && x.Member.NationalId == nationalId)
                .Select(x => new
                {
                    clinicAppt_id = x.ClinicListId,
                    日期 = x.Clinic.Date,
                    時段 = x.Clinic.ClinicTime.ClinicShifts,
                    科別 = x.Clinic.Doctor.Department,
                    醫師名稱 = x.Clinic.Doctor.Name,
                    診號 = x.ClinicNumber,
                    報到狀態 = x.PatientState.PatientStateName
                });

            await _hub.Clients.All.CardInsert(ip, memInfo.ToJson(), target.ToJson());

            return Ok();
        }

        [HttpGet]
        [Route("{controller}/{action}/{ip}")]
        public async Task<IActionResult> Remote_PullCard(string ip) 
        {
            await _hub.Clients.All.CardPull(ip);
            return Ok();
        }
    }
}
