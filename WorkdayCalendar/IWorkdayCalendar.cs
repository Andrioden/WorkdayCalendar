using System;
using System.Collections.Generic;
using System.Text;

namespace WorkdayCalendar
{
    public interface IWorkdayCalendar
    {
        void SetHoliday(DateTime date);
        void SetRecurringHoliday(int month, int day);
        void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes);

        DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays);
        string GetName();
    }
}
