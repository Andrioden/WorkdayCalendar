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
            // Convert the increment to a TimeSpan
            TimeSpan increment = TimeSpan.FromDays((double)incrementInWorkdays);

            // Set the start and end times for the workday
            DateTime startTime = startDate.Date.Add(_workdayStartTime);
            DateTime endTime = startDate.Date.Add(_workdayEndTime);

            // Adjust the start and end times for weekends and holidays
            while (!IsWorkday(startTime))
            {
                startTime = startTime.AddDays(1);
            }
            while (!IsWorkday(endTime))
            {
                endTime = endTime.AddDays(1);
            }

            // Calculate the total work time available in the workday
            TimeSpan workdayTime = endTime - startTime;

            // Subtract the workday start time from the start date to get the time remaining in the workday
            TimeSpan timeRemainingInWorkday = endTime - startDate;

            // Calculate the remaining work time in the workday
            TimeSpan remainingWorkTime = TimeSpan.Zero;
            if (timeRemainingInWorkday < workdayTime)
            {
                remainingWorkTime = workdayTime - timeRemainingInWorkday;
            }

            // Subtract the remaining work time from the increment time to get the elapsed work time
            TimeSpan elapsedWorkTime = increment - remainingWorkTime;

            // Add the elapsed work time to the end time to get the incremented date
            DateTime incrementedDate = endTime.Add(elapsedWorkTime);

            // Adjust the incremented date for weekends and holidays
            while (!IsWorkday(incrementedDate))
            {
                incrementedDate = incrementedDate.AddDays(1);
            }

            return incrementedDate;
        }

        public string GetName()
        {
            return "WorkdayCalendar";
        }
    }
}
