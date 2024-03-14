using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace ClinicWeb.Controllers
{
    public class FMemberBController : Controller
    {
        [Authorize(Policy = "frontendpolicy")]
        public IActionResult MemberIndex()
        {
            return View();
        }
    }


}
