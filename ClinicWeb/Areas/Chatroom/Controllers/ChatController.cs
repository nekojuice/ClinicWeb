using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Chatroom.Controllers
{
    [Area("Chatroom")]
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
