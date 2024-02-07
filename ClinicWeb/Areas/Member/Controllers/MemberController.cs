using ClinicWeb.Areas.Member.Models;
//要用自己生的模型
using ClinicWeb.Areas.Member.ViewModels;
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
        //老師建議：寫API可以拉一個獨立控制器
        //[HttpGet] Member/MemberGetdata
        //取得多筆會員資料
        [Route("{area}/{controller}/{action}")]
        public JsonResult MemberGetdata()
        {

            return Json(_context.MemberMemberList
                .Select(x => new
                {
                    會員id = x.MemberId,
                    會員編號 = x.MemberNumber,
                    姓名 = x.Name,
                    性別 = x.Gender ? "男" : "女",
                    血型 = x.BloodType,
                    身分證字號 = x.NationalId,
                    聯絡電話 = x.Phone,
                    地址 = x.Address,
                    緊急聯絡人 = x.IceName,
                    信箱 = x.MemEmail,


                    啟用 = (bool)x.Verification ? "啟用" : "未啟用"
                    //已經解決 性別不用這樣寫是為性別不允許null

                }
                ));
        }

        //顯示多筆會員資料
        public IActionResult MemIndex()
        {
            //可以指定不是這個名稱的view來顯示 return View("~Areas/Member/");
            return View();
        }
        //新增會員資料
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult MemberCreate( MemberViewModel memberMemberList)
        {
            //return Content("123");

            if (ModelState.IsValid)
            {
                //加入資料庫

                return RedirectToAction(nameof(MemIndex));
            }
            return Content("這段如果有放_ValidationScriptsPartial 就不易被觸發");

            //IEnumerable<MemberViewModel> memVM = _context.MemberMemberList.Select(member => new MemberViewModel
            //{
            //    // 將原始模型的屬性賦值給 ViewModel
            //    MemberIdVW = member.MemberId,
            //    MemberNumberVW = member.MemberNumber,
            //    NameVW = member.Name,
            //    GenderVW = member.Gender,
            //    // 以上為測試欄位
            //});
            //return View("MemberBasicInfo", memVM);


            //        return Json(_context.MemberMemberList
            //.Select(x => new
            //{
            //	會員編號 = "",
            //	姓名 = "",
            //	性別 = x.Gender ? "男" : "女",
            //	血型 = "",
            //	身分證字號 = "",
            //啟用=x.Verification

        }

        //修改會員資料的畫面顯示
        public IActionResult MemberEdit(int memberId)
        {
            var member = _context.MemberMemberList.Where(m => m.MemberId == memberId).FirstOrDefault();
            if (member == null) { return Content("123"); }
            MemberViewModel memVM = new MemberViewModel
            {
                // 將原始模型的屬性賦值給 ViewModel
                MemberIdVW = member.MemberId,
                MemberNumberVW = member.MemberNumber,
                NameVW = member.Name,
                GenderVW = member.Gender,
                BloodTypeVW = member.BloodType,
                NationalIdVW = member.NationalId,
                AddressVW = member.Address,
                ContactAddressVW = member.ContactAddress,
                PhoneVW = member.Phone,
                BirthDateVW = member.BirthDate,
                IceNameVW = member.IceName,
                MemPasswordVW = member.MemPassword,
                MemEmailVW = member.MemEmail,
                VerificationVW = member.Verification,


            };
            return PartialView("~/Areas/Member/Views/Partial/_MemberEditPartial.cshtml", memVM);
        }


        //把會員編輯好的資料送回資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,MemberNumber,Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,MemPassword,MemEmail,Verification,IsEnabled")] MemberMemberList member)
        {
            
            return View(member);
        }

    }
}
