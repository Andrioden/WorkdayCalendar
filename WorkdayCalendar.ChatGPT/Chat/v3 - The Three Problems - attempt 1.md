# Input
```
There are still three problems:
- _workdayStartTime dont exist, you need to remove the underscore
- _workdayEndTime dont exist, you need to remove the underscore
- IsWorkday dont exist
```

# Output
(ChatGPT Mar 23 Version)

I apologize for the confusion, here is the updated implementation with the corrections:

```csharp
using System;
using System.Collections.Generic;

namespace WorkdayNet
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        private List<DateTime> _holidayList;
        private List<DateTime> _recurringHolidayList;
        private TimeSpan _workdayStartTime;
        private TimeSpan _workdayEndTime;

        public WorkdayCalendar()
        {
            _holidayList = new List<DateTime>();
            _recurringHolidayList = new List<DateTime>();
        }

        public void SetHoliday(DateTime date)
        {
            _holidayList.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            DateTime date = new DateTime(DateTime.Now.Year, month, day);
            _recurringHolidayList.Add(date.Date);
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            _workdayStartTime = new TimeSpan(startHours, startMinutes, 0);
            _workdayEndTime = new TimeSpan(stopHours, stopMinutes, 0);
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            // Convert incrementInWorkdays to TimeSpan
            TimeSpan increment = TimeSpan.FromDays((double)incrementInWorkdays);

            // Calculate the end time including the increment
            DateTime endTime = startDate.Add(increment);

            // Check if end time is during a weekend or holiday
            while (!IsWorkday(endTime) || IsHoliday(endTime))
            {
                endTime = endTime.AddDays(1);
            }

            // Calculate the total work time between start and end time
            TimeSpan workTime = new TimeSpan();
            DateTime currentTime = startDate;
            while (currentTime < endTime)
            {
                // If the current time is during work hours, add to work time
                if (IsWorkday(currentTime) && !IsHoliday(currentTime) && IsWorkTime(currentTime.TimeOfDay))
                {
                    DateTime endOfWorkday = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, _workdayEndTime.Hours, _workdayEndTime.Minutes, 0);
                    TimeSpan timeLeftInWorkday = endOfWorkday - currentTime;

                    if (currentTime + timeLeftInWorkday > endTime)
                    {
                        workTime += endTime - currentTime;
                        currentTime = endTime;
                    }
                    else
                    {
                        workTime += timeLeftInWorkday;
                        currentTime += timeLeftInWorkday;
                    }
                }
                else
                {
                    // Move to the next day
                    currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0).AddDays(1);
                }
            }

            // Calculate the actual end time by adding the work time to start time
            DateTime actualEndTime = startDate.Add(workTime);

            // Check if actual end time is during a weekend or holiday
            while (!IsWorkday(actualEndTime) || IsHoliday(actualEndTime))
            {
                actualEndTime = actualEndTime.AddDays(1);
            }

            return actualEndTime;
        }

        private bool IsHoliday(DateTime date)
        {
            if (_holidayList.Contains(date.Date))
            {
                return true;
            }

            foreach (DateTime recurringHoliday in _recurringHolidayList)
            {
                if (recurringHoliday.Month == date.Month && recurringHoliday.Day == date.Day)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsWorkday(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        private bool IsWorkTime(TimeSpan time)
        {
```