using System;

namespace WorkdayCalendar
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        public void SetHoliday(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void SetRecurringHoliday(int month, int day)
        {
            throw new NotImplementedException();
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            throw new NotImplementedException();
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }
    }
}
