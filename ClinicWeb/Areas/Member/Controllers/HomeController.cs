﻿using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Member.Controllers
{
	[Area("Member")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
