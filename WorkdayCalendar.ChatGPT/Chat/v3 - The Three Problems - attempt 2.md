# Input
```
There are still three problems:
- _workdayStartTime dont exist, you need to remove the underscore
- _workdayEndTime dont exist, you need to remove the underscore
- IsWorkday dont exist
```

# Output

I apologize for the mistake in my previous response. Here is the corrected implementation:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkdayNet
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        private List<DateTime> _holidays = new List<DateTime>();
        private List<DateTime> _recurringHolidays = new List<DateTime>();
        private DateTime _workdayStartTime;
        private DateTime _workdayEndTime;

        public void SetHoliday(DateTime date)
        {
            if (!_holidays.Contains(date.Date))
            {
                _holidays.Add(date.Date);
            }
        }

        public void SetRecurringHoliday(int month, int day)
        {
            var recurringHoliday = new DateTime(DateTime.Now.Year, month, day);
            if (!_recurringHolidays.Contains(recurringHoliday))
            {
                _recurringHolidays.Add(recurringHoliday);
            }
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            _workdayStartTime = DateTime.Today.Add(new TimeSpan(startHours, startMinutes, 0));
            _workdayEndTime = DateTime.Today.Add(new TimeSpan(stopHours, stopMinutes, 0));
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            var workdaysToAdd = Math.Round(incrementInWorkdays, 2);
            var totalDaysToAdd = 0m;
            var currentDate = startDate.Date;

            if (workdaysToAdd > 0)
            {
                while (totalDaysToAdd < workdaysToAdd)
                {
                    currentDate = currentDate.AddDays(1);
                    if (IsWorkday(currentDate))
                    {
                        totalDaysToAdd += 1;
                    }
                }
            }
            else if (workdaysToAdd < 0)
            {
                while (totalDaysToAdd > workdaysToAdd)
                {
                    currentDate = currentDate.AddDays(-1);
                    if (IsWorkday(currentDate))
                    {
                        totalDaysToAdd -= 1;
                    }
                }
            }

            var remainingWorkdayFraction = workdaysToAdd - totalDaysToAdd;
            var remainingWorkdayDuration = TimeSpan.FromHours(remainingWorkdayFraction * (_workdayEndTime - _workdayStartTime).TotalHours);

            var result = currentDate.Add(_workdayStartTime.TimeOfDay + remainingWorkdayDuration);
            if (!IsWorkday(result))
            {
                result = GetNextWorkday(result);
            }

            return result;
        }

        private bool IsWorkday(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday
                && date.DayOfWeek != DayOfWeek.Sunday
                && !_holidays.Contains(date.Date)
                && !_recurringHolidays.Contains(new DateTime(DateTime.Now.Year, date.Month, date.Day)));
        }

        private DateTime GetNextWorkday(DateTime date)
        {
            while (!IsWorkday(date))
            {
                date = date.AddDays(1);
            }

            return date;
        }

        public string GetName()
        {
            return "WorkdayCalendar";
        }
    }
}
```

I hope this helps.