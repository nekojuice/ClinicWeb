using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.ClinicRoomSys.Controllers
{
    [Area("ClinicRoomSys")]
    
    public class CasesController : Controller
    {

        private readonly ClinicSysContext _context;
        public CasesController(ClinicSysContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpPost]
        [Route("{area}/{controller}/{action}/{ID}")]
        public JsonResult MSID(string ID)
        {
            return Json(_context.CasesMainCase.Where(x=>x.MemberId.ToString() == ID) );
        }
        //[HttpPut]
        //[HttpDelete]
    }
  
}
