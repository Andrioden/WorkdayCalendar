using System;
using System.Collections.Generic;
using System.Linq;
using WC.Core;

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
            bool isNegative = incrementInWorkdays < 0;
            incrementInWorkdays = Math.Abs(incrementInWorkdays);
            DateTime result = startDate;
            TimeSpan workdayDuration = _workdayEndTime - _workdayStartTime;
            int wholeWorkdays = (int)Math.Truncate(incrementInWorkdays);
            decimal remainingWorkdayFraction = incrementInWorkdays - wholeWorkdays;

            for (int i = 0; i < wholeWorkdays; i++)
            {
                do
                {
                    result = result.AddDays(isNegative ? -1 : 1);
                }
                while (!IsWorkday(result));
            }

            if (remainingWorkdayFraction > 0)
            {
                TimeSpan remainingWorkdayDuration = TimeSpan.FromTicks((long)(workdayDuration.Ticks * (double)remainingWorkdayFraction));
                TimeSpan elapsedTime = TimeSpan.Zero;

                do
                {
                    elapsedTime = result.TimeOfDay - _workdayStartTime;
                    if (isNegative)
                    {
                        elapsedTime = workdayDuration - elapsedTime;
                    }

                    if (elapsedTime < remainingWorkdayDuration)
                    {
                        result = result.AddDays(isNegative ? -1 : 1);
                        remainingWorkdayDuration -= elapsedTime;
                        result = result.Date + _workdayEndTime;
                    }
                    else
                    {
                        result = result.Date + (isNegative ? _workdayEndTime - remainingWorkdayDuration : _workdayStartTime + remainingWorkdayDuration);
                        break;
                    }
                }
                while (true);
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