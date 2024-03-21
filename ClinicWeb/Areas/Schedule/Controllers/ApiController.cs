//using ClinicWeb.Models;
//using AspNetCore;
using ClinicWeb.Areas.Schedule.Models;
//using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicWeb.Areas.Schedule.Controllers
{
    [Area("Schedule")]//路由設定123
    public class ApiController : Controller
    {


        private readonly ClinicSysContext _context;
        public ApiController(ClinicSysContext context)
        {
            _context = context;

        }


        public IActionResult SelectedDoctorName() //匯入醫師名稱到下拉選單
        {
            var doctornames = _context.MemberEmployeeList
                .Where(d => d.EmpType == "醫生" && d.Quit == true)
                .Select(c => new 
                { 
                    c.EmpId,
                    c.Name

                });
            return Json(doctornames);
        }

        [Route("{area}/{controller}/{action}/{year?}/{month?}")]
        public IActionResult ShowThismonthSchedule(string year, string month) //匯出當月醫師班表
        {
            if (string.IsNullOrEmpty(year))
            {
                year = DateTime.Now.Year.ToString();
            }
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.Month.ToString("D2");
            }

            var ThismonthSchedule = _context.ScheduleClinicInfo.Where(d => d.Date.StartsWith($"{year}/{month}"))
                .Select(x => new
                {
                    //id = x.ClinicInfoId,
                    timeid = x.ClinicTimeId,
                    日期 = x.Date,
                    醫師 = x.Doctor.Name,
                    時段 = x.ClinicTime.ClinicShifts,
                    科別 = x.Doctor.Department,
                    診間 = x.ClincRoom.Name,
                    掛號上限 = x.RegistrationLimit
                });
            //.OrderBy(d=>d.日期);
            return Json(ThismonthSchedule);
        }

        //匯出月份
        public IActionResult GetMonth()
        {
            var month = _context.ScheduleClinicInfo.Select(d => d.Date.Substring(0, 7)).Distinct().OrderBy(m => m); //2023/12
            return Json(month);

        }

        //匯出班表時段
        public IActionResult GetShifts()
        {
            var month = _context.ScheduleClinicInfo
                .Select(d => new
                {
                    d.ClinicTime.ClinicTimeId,
                    d.ClinicTime.ClinicShifts
                })
                .OrderBy(o => o.ClinicTimeId).Distinct();

            return Json(month);

        }

        //匯出科別
        public IActionResult GetDepartment()
        {
            var department = _context.ScheduleClinicInfo
                .Select(d => d.Doctor.Department).Distinct();


            return Json(department);

        }

        //匯出當年月份
        public IActionResult GetNowMonth()
        {
            List<string> months = new List<string>();
            for (int month = 1; month <= 12; month++)
            {
                string monthString = $"{DateTime.Now.Year}/{month.ToString().PadLeft(2, '0')}";
                months.Add(monthString);
            }
            return Json(months);

        }

        [Route("{area}/{controller}/{action}/{year}/{month}")]
        //排班
        public IActionResult Scheduling(string year,string month)
        {
            string start = year + "/" + month;    //假設排2023-12月班表
            // 檢查重複排班
            bool hasExistingSchedule = _context.ScheduleClinicInfo.Any(s => s.Date.StartsWith(start));

            if (hasExistingSchedule)
            {
                return BadRequest("選擇月份已存在班表，請勿重複排班。");
            }

            var result = _context.ScheduleClinicSchedule.Select(x => new
            {
                醫師ID = x.DoctorId,
                科別 = x.Doctor.Department,
                診間ID = x.RoomId,
                時段ID = x.TimeId,
                星期 = x.Week
            });


            ////取得排班起始日
            ////用textbox, combobox, DateTime.Now...等其他方法決定start日期
            

            string week = DateTime.Now.DayOfWeek.ToString("");    //取得今天 星期
            string startWeek = DateTime.Parse(start).DayOfWeek.ToString();  //1號的星期(英文) //五

            foreach (var item in result)    //1. 對門診表新增
            {
                var dict = daysInMonthOnWeek(DateTime.Parse(start).Year, DateTime.Parse(start).Month);//取得星期-日期對照表


                foreach (int day in dict[(int)item.星期]) //如果抽到禮拜三: (int day in {6, 13, 20, 27})
                {
                    var newinfo = new ScheduleClinicInfo()  
                    {

                        DoctorId = item.醫師ID,
                        ClincRoomId = item.診間ID,
                        Date = start +"/"+ day.ToString("00"),  //"2023-12-" + "01" ("00"為格式，個位數會補0)
                        ClinicTimeId = item.時段ID,
                        RegistrationLimit = 30,
                        JumpStatus = 0,
                        LeaveStatus = 0
                    };
                    _context.ScheduleClinicInfo.Add(newinfo);   //加入資料表
                }
            }

            _context.SaveChanges();
            return Ok();

            }

        private Dictionary<int, int[]> daysInMonthOnWeek(int year, int month)//日期對照表
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);    //這個月到幾號    //31
            int weekOfFirstDay = intWeek(new DateTime(year, month, 1).DayOfWeek);        //這個月一號是禮拜幾    //5
            Dictionary<int, int[]> dict = new Dictionary<int, int[]>(); //建立對照表 星期幾0~6 <===> 日期陣列
            List<int> sun = new List<int>();
            List<int> mon = new List<int>();
            List<int> tue = new List<int>();
            List<int> wed = new List<int>();
            List<int> thu = new List<int>();
            List<int> fri = new List<int>();
            List<int> sat = new List<int>();

            int dayCounter = 1;    //建立day計數器，直到這個月最後一天
            while (dayCounter <= daysInMonth)    //從1記到最後一天  (12.1~12.31)
            {
                for (int i = 0; i <= 6; i++)    //指針是i，依序從0填到6
                {
                    if (dayCounter == 1)
                    { i = weekOfFirstDay; }//如果是第一天，把指針移動到該星期 (2023.12.1 要填入 5=星期五)

                    if (dayCounter <= daysInMonth)
                    {
                        switch (i)
                        {
                            case 0: sun.Add(dayCounter); break;  //幾號 是 星期幾，填入
                            case 1: mon.Add(dayCounter); break;
                            case 2: tue.Add(dayCounter); break;
                            case 3: wed.Add(dayCounter); break;
                            case 4: thu.Add(dayCounter); break;
                            case 5: fri.Add(dayCounter); break;
                            case 6: sat.Add(dayCounter); break;
                        }
                        dayCounter++;
                    }
                }
            }
            dict.Add(0, sun.ToArray());
            dict.Add(1, mon.ToArray());
            dict.Add(2, tue.ToArray());
            dict.Add(3, wed.ToArray());
            dict.Add(4, thu.ToArray());
            dict.Add(5, fri.ToArray());
            dict.Add(6, sat.ToArray());

            return dict;
        }

        private int intWeek(DayOfWeek date)   //翻譯星期名成數字，才能i++
        {
            int week = 0;
            switch (date)
            {
                case DayOfWeek.Sunday: week = 0; break;
                case DayOfWeek.Monday: week = 1; break;
                case DayOfWeek.Tuesday: week = 2; break;
                case DayOfWeek.Wednesday: week = 3; break;
                case DayOfWeek.Thursday: week = 4; break;
                case DayOfWeek.Friday: week = 5; break;
                case DayOfWeek.Saturday: week = 6; break;
            }
            return week;
        }

    }



    }

