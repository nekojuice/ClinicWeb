using Microsoft.AspNetCore.Mvc;

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
