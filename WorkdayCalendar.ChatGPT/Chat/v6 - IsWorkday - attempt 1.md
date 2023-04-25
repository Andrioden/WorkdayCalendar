# Input
```
I noticed another problem, which stops it from compiling, IsWorkday has one to many ) at the end
```

# Output

Apologies for the mistake. Here's the corrected version of the code:

```
using System;
using System.Collections.Generic;

namespace WorkdayNet
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        private HashSet<DateTime> _holidays = new HashSet<DateTime>();
        private HashSet<int> _recurringHolidays = new HashSet<int>();
        private TimeSpan _workdayStartTime;
        private TimeSpan _workdayEndTime;

        public void SetHoliday(DateTime date)
        {
            _holidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            _recurringHolidays.Add(DateTime.Parse($"{month}-{day}").DayOfYear);
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            _workdayStartTime = new TimeSpan(startHours, startMinutes, 0);
            _workdayEndTime = new TimeSpan(stopHours, stopMinutes, 0);
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            // Determine direction of increment (forward or backward)
            int incrementDirection = incrementInWorkdays < 0 ? -1 : 1;

            // Get integer part of incrementInWorkdays
            int integerDays = (int)Math.Truncate(Math.Abs(incrementInWorkdays));

            // Get remaining workday fraction of incrementInWorkdays
            decimal remainingWorkdayFraction = Math.Abs(incrementInWorkdays) - integerDays;

            // Initialize result
            DateTime result = startDate.Date;

            // Add integer number of workdays
            int addedDays = 0;
            while (addedDays < integerDays)
            {
                result = result.AddDays(incrementDirection);

                if (IsWorkday(result))
                {
                    addedDays++;
                }
            }

            // Add remaining workday fraction
            if (remainingWorkdayFraction > 0)
            {
                TimeSpan workdayDuration = _workdayEndTime - _workdayStartTime;
                double remainingWorkdayDuration = (double)(workdayDuration.TotalHours * (double)remainingWorkdayFraction);
                TimeSpan remainingTime = TimeSpan.FromHours(remainingWorkdayDuration);

                if (incrementDirection == 1)
                {
                    TimeSpan timeLeftInWorkday = _workdayEndTime - startDate.TimeOfDay;
                    if (timeLeftInWorkday >= remainingTime)
                    {
                        result = startDate + remainingTime;
                    }
                    else
                    {
                        result = result.AddDays(1).Date + _workdayStartTime + remainingTime - timeLeftInWorkday;
                    }
                }
                else if (incrementDirection == -1)
                {
                    TimeSpan timeLeftInWorkday = startDate.TimeOfDay - _workdayStartTime;
                    if (timeLeftInWorkday >= remainingTime)
                    {
                        result = startDate - remainingTime;
                    }
                    else
                    {
                        result = result.AddDays(-1).Date + _workdayEndTime - remainingTime + timeLeftInWorkday;
                    }
                }
            }

            return result;
        }

        public string GetName()
        {
            return "WorkdayCalendar";
        }

        private bool IsWorkday(DateTime date)
        {
            // Check if date is a holiday
            if (_holidays.Contains(date.Date))
            {
                return false;
            }

            // Check if date is a recurring holiday
            if (_recurringHolidays.Contains(date.DayOfYear))
            {
                return false;
            }

            // Check if date is a weekend
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // Check if date is during workday
```