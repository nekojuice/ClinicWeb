using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
//using ClinicWeb.Models;
using ClinicWeb.Areas.Member.Models;



namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    public class MBMemberInfoController : Controller
    {
        private ClinicSysContext _context;
        public IActionResult Index()
        {
            var user = HttpContext.User;
            var memberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;

            if (string.IsNullOrEmpty(memberID))
            {
                return Content("未登入", "application/json");
            }

            var memberInfo = _context.MemberMemberList
                                .FirstOrDefault(m => m.MemberId == Convert.ToInt32(memberID));
            
            
            if (memberInfo == null)
            {
                return Content("找不到會員資料", "application/json");
            }


            return View(memberInfo);
        }

        public MBMemberInfoController(ClinicSysContext context)
        {
            _context = context;
        }
        public IActionResult MemberProfile()
        {
            return PartialView("~/Views/FMemberB/PartialView/_MemberProfilePartial.cshtml");
        }

        //編輯資料時檢查該筆會員id是否存在的函數
        private bool MemberMemberListExists(int memberId)
        {
            return _context.MemberMemberList.Any(e => e.MemberId == memberId);
        }

        // 修改會員資料
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(MemberMemberList member)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(member);
        //            await _context.SaveChangesAsync();

        //            // 修改成功，顯示 SweetAlert
        //            TempData["Success"] = "資料修改成功！"; // 在 TempData 中存儲成功消息
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MemberMemberListExists(member.MemberId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return Content("驗證未通過");
        //    }

        //    // 取得剛剛那筆會員資料的 ID
        //    int memberId = member.MemberId;
        //    //return View("~/Areas/Member/Views/_MemberIndex.cshtml",  member.MemberId);
        //    return MemberGetdataOne(memberId);
        //}
        [HttpPost]
        public async Task<IActionResult> Edit(MemberMemberList member)
        {

            var memberIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "MemberID");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim.Value, out int memberIdFromClaim))
            {
                return BadRequest("系統異常，找不到登入會員");
            }
            // 驗證傳入的會員ID與Claim中的會員ID是否匹配
            if (member.MemberId != memberIdFromClaim)
            {
                return BadRequest("系統異常，操作的會員ID與登入會員不匹配");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();

                    var updatedMember = MemberGetdataOne(member.MemberId); // 直接調用
                    return updatedMember; // 返回 JsonResult
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
                return Json(new { error = "驗證未通過" }); // 返回錯誤訊息
            }
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
                    性別 = (bool)x.Gender ? "男" : "女",
                    血型 = x.BloodType,
                    身分證字號 = x.NationalId,
                    聯絡電話 = x.Phone,
                    地址 = x.Address,
                    緊急聯絡人 = x.IceName,
                    信箱 = x.MemEmail,


                    //啟用 = (bool)x.Verification ? "啟用" : "未啟用"
                })
                .FirstOrDefault()
                );
        }

    }
}
