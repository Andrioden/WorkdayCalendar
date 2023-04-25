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
            TimeSpan workdayStart = _workdayStartTime.TimeOfDay;
            TimeSpan workdayEnd = _workdayEndTime.TimeOfDay;
            TimeSpan workdayDuration = workdayEnd - workdayStart;

            decimal remainingWorkdayFraction = (decimal)(startDate.TimeOfDay - workdayStart).Ticks / workdayDuration.Ticks;
            remainingWorkdayFraction = Math.Min(Math.Max(remainingWorkdayFraction, 0m), 1m);

            DateTime currentDate = startDate.Date;
            while (incrementInWorkdays != 0)
            {
                int direction = Math.Sign(incrementInWorkdays);
                TimeSpan incrementTime = TimeSpan.FromTicks((long)(Math.Abs(incrementInWorkdays) * workdayDuration.Ticks));
                while (incrementTime.Ticks > 0)
                {
                    if (direction > 0)
                    {
                        currentDate = currentDate.AddDays(1);
                        if (IsWorkday(currentDate))
                        {
                            TimeSpan timeToAdd = workdayEnd - workdayStart;
                            if (incrementTime >= timeToAdd)
                            {
                                incrementTime -= timeToAdd;
                                remainingWorkdayFraction = 0;
                            }
                            else
                            {
                                remainingWorkdayFraction = (decimal)incrementTime.Ticks / workdayDuration.Ticks;
                                incrementTime = TimeSpan.Zero;
                            }
                        }
                    }
                    else if (direction < 0)
                    {
                        if (IsWorkday(currentDate))
                        {
                            TimeSpan timeToSubtract = workdayEnd - workdayStart;
                            if (incrementTime >= timeToSubtract)
                            {
                                incrementTime -= timeToSubtract;
                                remainingWorkdayFraction = 1;
                            }
                            else
                            {
                                remainingWorkdayFraction = 1 - (decimal)incrementTime.Ticks / workdayDuration.Ticks;
                                incrementTime = TimeSpan.Zero;
                            }
                        }
                        currentDate = currentDate.AddDays(-1);
                    }
                }
                incrementInWorkdays -= direction;
            }

            TimeSpan remainingTime = TimeSpan.FromTicks((long)(remainingWorkdayFraction * workdayDuration.Ticks));
            DateTime result = currentDate.Date + remainingTime;
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