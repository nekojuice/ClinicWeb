using ClinicWeb.Areas.Member.Models;
//要用自己生的模型
using ClinicWeb.Areas.Member.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;


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

        //修改完資料後抓取單筆資料
        [Route("{area}/{controller}/{action}/{memberId}")]
        [HttpPost]
        public JsonResult MemberGetdataOne(int memberId)
        {
            return Json(_context.MemberMemberList
                .Where(x => x.MemberId == Convert.ToInt32(memberId))
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
                })
                .FirstOrDefault()
                );
        }

        //顯示多筆會員資料
        public IActionResult MemIndex()
        {
            //可以指定不是這個名稱的view來顯示 return View("~Areas/Member/");
            return View();
        }
        //新增會員資料
        [HttpPost]
        [ValidateAntiForgeryToken]
		//[Bind("Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,IceNumber,MemPassword,MemEmail,Verification")]
		public IActionResult MemberCreate([Bind("Name,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,IceNumber,MemPassword,MemEmail,Verification")] [FromBody] MemberMemberList member)
        {

            if (member == null)
            {
                Console.WriteLine("null~~");
            }
            else
            {
                Console.WriteLine(member.Name);
                Console.WriteLine(member.Gender);
                Console.WriteLine(member.BloodType);
                Console.WriteLine(member.NationalId);
                Console.WriteLine(member.Address);
                Console.WriteLine(member.ContactAddress);
                Console.WriteLine(member.Phone);
                Console.WriteLine(member.BirthDate);
                Console.WriteLine(member.IceName);
                Console.WriteLine(member.IceNumber);
                Console.WriteLine(member.MemPassword);
                Console.WriteLine(member.MemEmail);
                Console.WriteLine(member.Verification);
            }
            
            //return Content("123");
            //檢查驗證
            if (!ModelState.IsValid)
            {
				// 如果模型驗證失敗，重新顯示包含錯誤信息的表單
				Console.WriteLine("驗證失敗");
				return PartialView("~/Areas/Member/Views/Partial/_MemberCreatePartial.cshtml", member); // 返回相同的視圖，這將會顯示錯誤信息
                //return Json(member);
            }
            else
            {
                //加入資料庫
   
                var maxMemberNumber = _context.MemberMemberList.Max(m => m.MemberNumber);
                var nextMemberNumber = maxMemberNumber + 1;
                member.MemberNumber = nextMemberNumber;

                _context.MemberMemberList.Add(member);
                _context.SaveChanges();
                return Content("success"); 
            }

        }



        //修改會員資料的畫面顯示
        public async Task<IActionResult> MemberEdit(int? memberId)
        {
            if (memberId == null || _context.MemberMemberList == null)
            {
                return NotFound();
            }

            var memberList = await _context.MemberMemberList.FindAsync(memberId);
            if (memberList == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Member/Views/Partial/_MemberEditPartial.cshtml", memberList);
            //return View(memberEmployeeList);

            //var member = _context.MemberMemberList.Where(m => m.MemberId == memberId).FirstOrDefault();

            //if (member == null) { return Content("123"); } 
            //MemberViewModel memVM = new MemberViewModel
            //{
            //    // 將原始模型的屬性賦值給 ViewModel
            //    MemberIdVW = member.MemberId,
            //    MemberNumberVW = member.MemberNumber,
            //    NameVW = member.Name,
            //    GenderVW = member.Gender,
            //    BloodTypeVW = member.BloodType,
            //    NationalIdVW = member.NationalId,
            //    AddressVW = member.Address,
            //    ContactAddressVW = member.ContactAddress,
            //    PhoneVW = member.Phone,
            //    BirthDateVW = member.BirthDate,
            //    IceNameVW = member.IceName,
            //    MemPasswordVW = member.MemPassword,
            //    MemEmailVW = member.MemEmail,
            //    VerificationVW = member.Verification,


            //};
            //return PartialView("~/Areas/Member/Views/Partial/_MemberEditPartial.cshtml", member);

        }




        //編輯資料時檢查該筆會員id是否存在的函數
        private bool MemberMemberListExists(int memberId)
        {
            return _context.MemberMemberList.Any(e => e.MemberId == memberId);
        }


        //把會員編輯好的資料送回資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        /* [Bind("MemberId,MemberNumber,Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,MemPassword,MemEmail,Verification,IsEnabled")]*/
        public async Task<IActionResult> Edit([Bind("MemberId,MemberNumber,Name,Verification")] int MemberId, MemberMemberList member)
        {
            if (MemberId != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
           
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberMemberListExists(member.MemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
             
            }
            else
            {
                return Content("驗證未通過");
            }

            // 取得剛剛那筆會員資料的 ID
            int memberId = member.MemberId;
            //return View("~/Areas/Member/Views/_MemberIndex.cshtml",  member.MemberId);
            return MemberGetdataOne(memberId);
        }

    }
}
