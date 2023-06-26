using System;
using System.Collections.Generic;
using System.Text;

namespace FindJobs.App.helper
{
    public static class DayCalculate
    {
        public static string GetTextOfDay(DateTime date)
        {
            var currentDate = DateTime.Now;
            var day = currentDate.Day - date.Day;
            if (day == 0)
            {
                return Res.CompanyOffer.ToDay;
            }
            
            if (day == 1)
            {
                return Res.CompanyOffer.OneDayAgo;
            }
            else if (day == 2)
            {
                return Res.CompanyOffer.TwoDaysAgo;
            }
            else if (day == 3)
            {
                return Res.CompanyOffer.ThreeDaysAgo;
            }
            else if (day > 3 && day < 8)
            {
                return Res.CompanyOffer.OneWeeksAgo;
            }
            else if (day > 7 && day < 15)
            {
                return Res.CompanyOffer.TwoWeeksAgo;
            }
            else if (day > 15)
            {
                return Res.CompanyOffer.OneMonthAgo;
            }
            else if (day > 31)
            {
                return Res.CompanyOffer.TwoMonthsAgo;
            }
            return null;
        }
    }
}
