using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace ClinicWeb.Controllers
{
    public class FMemberBController : Controller
    {
        public IActionResult MemberIndex()
        {
            return View();
        }

        public IActionResult MemberProfile()
        {
            return PartialView("~/Views/FMemberB/PartialView/_MemberProfilePartial.cshtml");
        }
    }


}
