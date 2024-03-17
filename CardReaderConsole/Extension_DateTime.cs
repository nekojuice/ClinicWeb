using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIT155_E_MID.ApptSystem.Extension
{
    public static class Extension_DateTime
    {
        public static int intWeek(this DayOfWeek date)   //翻譯星期名成數字，才能i++
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

        public static string chineseWeek(this DayOfWeek date)   //翻譯星期名成數字，才能i++
        {
            string week = "";
            switch (date)
            {
                case DayOfWeek.Sunday: week = "日"; break;
                case DayOfWeek.Monday: week = "一"; break;
                case DayOfWeek.Tuesday: week = "二"; break;
                case DayOfWeek.Wednesday: week = "三"; break;
                case DayOfWeek.Thursday: week = "四"; break;
                case DayOfWeek.Friday: week = "五"; break;
                case DayOfWeek.Saturday: week = "六"; break;
            }
            return week;
        }

        public static string TWdateFormate(string twdate)
        {
            string twY = twdate.Substring(0, 3);
            string twM = twdate.Substring(3, 2);
            string twD = twdate.Substring(5, 2);

            string formatY = (Convert.ToInt32(twY) + 1911).ToString();
            return $"{formatY}/{twM}/{twD}";
        }
    }
}
