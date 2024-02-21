using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Areas.Member.Models;

namespace ClinicWeb.Areas.Member.Controllers
{
    [Area("Member")]
    public class EmpController : Controller
    {
        private readonly ClinicSysContext _context;

        public EmpController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult EmpIndex()
        {
            return View("EmpIndex");
            //return View("Index2");
        }
       

        [Route("{area}/{controller}/{action}")]
        public JsonResult EmpGetdata()
        {

            return Json(_context.MemberEmployeeList
                .Select(x => new
                {
                    員工id = x.EmpId,
                    員工編號 = x.StaffNumber,
                    姓名 = x.Name,
                    性別 = (bool)x.Gender ? "男" : "女",
                    血型 = x.BloodType,
                    身分證字號 = x.NationalId,
                    生日= ((DateTime)x.BirthDate).ToString("yyyy-MM-dd"),
                    聯絡電話 = x.Phone,
                    地址=x.Address,
                    員工類別=x.EmpType,


                    在職 =(bool)x.Quit ? "在職" : "離職",
                

            
                    //已經解決 性別不用這樣寫是為性別不允許null

                }
                ));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,IceNumber,MemPassword,MemEmail,Verification")]
        public IActionResult EmpCreate([Bind("Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,EmpPassword,EmpType,Department,Quit")][FromBody] MemberEmployeeList emp)
        {

            if (emp == null)
            {
                Console.WriteLine("null~~");
            }
            else
            {
                Console.WriteLine(emp.Name);
                Console.WriteLine(emp.Gender);
                Console.WriteLine(emp.BloodType);
                Console.WriteLine(emp.NationalId);
                Console.WriteLine(emp.Address);
                Console.WriteLine(emp.ContactAddress);
                Console.WriteLine(emp.Phone);
                Console.WriteLine(emp.BirthDate);
                Console.WriteLine(emp.StaffNumber);
                Console.WriteLine(emp.Department);
                Console.WriteLine(emp.Quit);
            }

            //檢查驗證
            if (!ModelState.IsValid)
            {
                // 如果模型驗證失敗，重新顯示包含錯誤信息的表單
                Console.WriteLine("驗證失敗");
                return PartialView("~/Areas/Member/Views/Partial/_EmpCreatePartial.cshtml", emp); // 返回相同的視圖，這將會顯示錯誤信息
                //return Json(member);
            }
            else
            {
                //加入資料庫

                var maxEmpNumber = _context.MemberEmployeeList.Max(m => m.StaffNumber);
                var nextEmpNumber = maxEmpNumber + 1;
                emp.StaffNumber = nextEmpNumber;

                _context.MemberEmployeeList.Add(emp);
                _context.SaveChanges();
                return Content("success");
            }

        }
        public async Task<IActionResult> EmpEdit(int? empId)
        {
            if (empId == null || _context.MemberEmployeeList == null)
            {
                return NotFound();
            }

            var empList = await _context.MemberEmployeeList.FindAsync(empId);
            if (empList == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Member/Views/Partial/_EmpEditPartial.cshtml", empList);
        }

		private bool MemberMemberListExists(int empId)
		{
			return _context.MemberEmployeeList.Any(e => e.EmpId == empId);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		/* [Bind("MemberId,MemberNumber,Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,MemPassword,MemEmail,Verification,IsEnabled")]*/
		public async Task<IActionResult> Edit(int empId, MemberEmployeeList emp)
		{
			if (empId != emp.EmpId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{

					_context.Update(emp);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!MemberMemberListExists(emp.EmpId))
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
			int memberId = emp.EmpId;
			//return View("~/Areas/Member/Views/_MemberIndex.cshtml",  member.MemberId);
			return EmpGetdataOne(memberId);
		}

		[Route("{area}/{controller}/{action}/{empId}")]
		[HttpPost]
		public JsonResult EmpGetdataOne(int empId)
		{
			return Json(_context.MemberEmployeeList
				.Where(x => x.EmpId == Convert.ToInt32(empId))
				.Select(x => new
				{
					員工id = x.EmpId,
					員工編號 = x.StaffNumber,
					姓名 = x.Name,
					性別 = (bool)x.Gender ? "男" : "女",
					血型 = x.BloodType,
					身分證字號 = x.NationalId,
					生日 = ((DateTime)x.BirthDate).ToString("yyyy-MM-dd"),
					聯絡電話 = x.Phone,
					地址 = x.Address,
					員工類別 = x.EmpType,


					在職 = (bool)x.Quit ? "在職" : "離職",
				})
				.FirstOrDefault()
				);
		}

		//// GET: Member/Emp
		//public async Task<IActionResult> Index()
		//{
		//    return  View() ;

		//}

		//// GET: Member/Emp/Details/5
		//public async Task<IActionResult> Details(int? id)
		//{
		//    if (id == null || _context.MemberEmployeeList == null)
		//    {
		//        return NotFound();
		//    }

		//    var memberEmployeeList = await _context.MemberEmployeeList
		//        .FirstOrDefaultAsync(m => m.EmpId == id);
		//    if (memberEmployeeList == null)
		//    {
		//        return NotFound();
		//    }

		//    return View(memberEmployeeList);
		//}

		//// GET: Member/Emp/Create
		//public IActionResult Create()
		//{
		//    return View();
		//}

		//// POST: Member/Emp/Create
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("EmpId,StaffNumber,Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,EmpType,Department,EmpPassword,EmpPhoto,Quit")] MemberEmployeeList memberEmployeeList)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        _context.Add(memberEmployeeList);
		//        await _context.SaveChangesAsync();
		//        return RedirectToAction(nameof(Index));
		//    }
		//    return View(memberEmployeeList);
		//}

		//// GET: Member/Emp/Edit/5
		//public async Task<IActionResult> Edit(int? id)
		//{
		//    if (id == null || _context.MemberEmployeeList == null)
		//    {
		//        return NotFound();
		//    }

		//    var memberEmployeeList = await _context.MemberEmployeeList.FindAsync(id);
		//    if (memberEmployeeList == null)
		//    {
		//        return NotFound();
		//    }
		//    return View(memberEmployeeList);
		//}

		//// POST: Member/Emp/Edit/5
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("EmpId,StaffNumber,Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,EmpType,Department,EmpPassword,EmpPhoto,Quit")] MemberEmployeeList memberEmployeeList)
		//{
		//    if (id != memberEmployeeList.EmpId)
		//    {
		//        return NotFound();
		//    }

		//    if (ModelState.IsValid)
		//    {
		//        try
		//        {
		//            _context.Update(memberEmployeeList);
		//            await _context.SaveChangesAsync();
		//        }
		//        catch (DbUpdateConcurrencyException)
		//        {
		//            if (!MemberEmployeeListExists(memberEmployeeList.EmpId))
		//            {
		//                return NotFound();
		//            }
		//            else
		//            {
		//                throw;
		//            }
		//        }
		//        return RedirectToAction(nameof(Index));
		//    }
		//    return View(memberEmployeeList);
		//}

		//// GET: Member/Emp/Delete/5
		//public async Task<IActionResult> Delete(int? id)
		//{
		//    if (id == null || _context.MemberEmployeeList == null)
		//    {
		//        return NotFound();
		//    }

		//    var memberEmployeeList = await _context.MemberEmployeeList
		//        .FirstOrDefaultAsync(m => m.EmpId == id);
		//    if (memberEmployeeList == null)
		//    {
		//        return NotFound();
		//    }

		//    return View(memberEmployeeList);
		//}

		//// POST: Member/Emp/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> DeleteConfirmed(int id)
		//{
		//    if (_context.MemberEmployeeList == null)
		//    {
		//        return Problem("Entity set 'ClinicSysContext.MemberEmployeeList'  is null.");
		//    }
		//    var memberEmployeeList = await _context.MemberEmployeeList.FindAsync(id);
		//    if (memberEmployeeList != null)
		//    {
		//        _context.MemberEmployeeList.Remove(memberEmployeeList);
		//    }

		//    await _context.SaveChangesAsync();
		//    return RedirectToAction(nameof(Index));
		//}

		//private bool MemberEmployeeListExists(int id)
		//{
		//  return (_context.MemberEmployeeList?.Any(e => e.EmpId == id)).GetValueOrDefault();
		//}
	}
}
