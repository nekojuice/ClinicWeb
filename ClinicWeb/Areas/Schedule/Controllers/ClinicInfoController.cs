using ClinicWeb.Areas.Schedule.Models;
using Microsoft.AspNetCore.Mvc;

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


    }
}
