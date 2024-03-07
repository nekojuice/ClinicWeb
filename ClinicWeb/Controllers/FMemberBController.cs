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
    }


}
