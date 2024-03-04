using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class MBMemberInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MemberProfile()
        {
            return PartialView("~/Views/FMemberB/PartialView/_MemberProfilePartial.cshtml");
        }
    }
}
