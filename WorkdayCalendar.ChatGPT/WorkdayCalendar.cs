using System;
using System.Collections.Generic;
using WC.Core;

namespace WorkdayNet
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        private HashSet<DateTime> holidays;
        private HashSet<DateTime> recurringHolidays;
        private TimeSpan workdayStart;
        private TimeSpan workdayStop;

        public WorkdayCalendar()
        {
            holidays = new HashSet<DateTime>();
            recurringHolidays = new HashSet<DateTime>();
            workdayStart = new TimeSpan(8, 0, 0);
            workdayStop = new TimeSpan(16, 0, 0);
        }

        public void SetHoliday(DateTime date)
        {
            holidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            recurringHolidays.Add(new DateTime(2000, month, day));
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            workdayStart = new TimeSpan(startHours, startMinutes, 0);
            workdayStop = new TimeSpan(stopHours, stopMinutes, 0);
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            TimeSpan increment = TimeSpan.FromDays((double)incrementInWorkdays);

            // Adjust increment for workday start and stop times
            if (incrementInWorkdays > 0)
            {
                if (startDate.TimeOfDay >= workdayStop)
                {
                    startDate = startDate.Date + workdayStart;
                }
                increment -= workdayStop - startDate.TimeOfDay;
            }
            else if (incrementInWorkdays < 0)
            {
                if (startDate.TimeOfDay < workdayStart)
                {
                    startDate = startDate.Date + workdayStop.AddDays(-1);
                }
                increment -= startDate.TimeOfDay - workdayStart;
            }

            // Calculate the new date, skipping weekends and holidays
            while (increment.Ticks > 0)
            {
                startDate = startDate.AddDays(1);
                if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }
                if (holidays.Contains(startDate) || recurringHolidays.Contains(new DateTime(2000, startDate.Month, startDate.Day)))
                {
                    continue;
                }
                increment -= TimeSpan.FromDays(1);
            }
            while (increment.Ticks < 0)
            {
                startDate = startDate.AddDays(-1);
                if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }
                if (holidays.Contains(startDate) || recurringHolidays.Contains(new DateTime(2000, startDate.Month, startDate.Day)))
                {
                    continue;
                }
                increment += TimeSpan.FromDays(1);
            }

            // Adjust date for workday start and stop times
            if (incrementInWorkdays > 0 && startDate.TimeOfDay < workdayStart)
            {
                startDate = startDate.Date + workdayStart;
            }
            else if (incrementInWorkdays < 0 && startDate.TimeOfDay >= workdayStop)
            {
                startDate = startDate.Date + workdayStop.AddDays(-1);
            }
            return startDate + increment;
        }

        public string GetName()
        {
            return "WorkdayCalendar";
        }
    }
}
