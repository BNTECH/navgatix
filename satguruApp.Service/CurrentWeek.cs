using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service
{
    public static class CurrentWeek
    {
        public static string GetCurrentWeek()
        {
            var currentDate = DateTime.UtcNow;
            CultureInfo cul = CultureInfo.CurrentCulture;
            int weekNum = cul.Calendar.GetWeekOfYear(
                currentDate,
                CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);
            int year = weekNum == 52 && currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;
            string weekYear = year + weekNum.ToString();
            return weekYear;
        }
    }
}
