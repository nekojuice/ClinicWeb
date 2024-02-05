using ClinicWeb.Areas.Member.Models;
//要用自己生的模型
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ClinicWeb.Areas.Member.Controllers
{
    [Area("Member")]
    public class MemberController : Controller
    {

        private ClinicSysContext _context;
        public MemberController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        //API拉一個獨立控制器
        [Route("{area}/{controller}/{action}")]
        //[HttpGet]
        public JsonResult MemberGetdata() 
        {
            
            return Json(_context.MemberMemberList
                .Select(x => new
                {
                    //會員id=x.MemberId,
                    會員編號 = x.MemberNumber,
                    姓名 = x.Name,
                    性別=x.Gender?"男":"女",
                    血型 = x.BloodType,
                    身分證字號 =x.NationalId,
                    聯絡電話 = x.Phone,
					地址 = x.Address,
                    緊急聯絡人 = x.IceName,
                    信箱 = x.MemEmail,


                    啟用 = (bool)x.Verification ? "啟用" : "未啟用"          
                    //已經解決 性別不用這樣寫是為性別不允許null

                }
				));
        }

        public IActionResult MemIndex()
        {
            //可以指定不是這個名稱的view來顯示 return View("~Areas/Member/");
            return View();
        }
        //新增會員資料
		public JsonResult MemberCreate()
		{

			return Json(_context.MemberMemberList
				.Select(x => new
				{
					會員編號 = "",
					姓名 = "",
					性別 = x.Gender ? "男" : "女",
					血型 = "",
					身分證字號 = "",
					//啟用=x.Verification

				}
				));
		}




	}
}
