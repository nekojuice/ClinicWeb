using ClinicWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    public class FChattingRoomController : Controller
	{
        
        public IActionResult FChattingRoom(MemberMemberList user)
		{
			return View(user);
		}
	}
}
