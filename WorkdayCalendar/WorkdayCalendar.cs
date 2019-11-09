using System;
using System.Collections.Generic;

namespace WorkdayCalendar
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        private List<DateTime> Holidays = new List<DateTime>();


        /// <summary>
        /// Add this date to the list of holidays. Only the .Date component is considered.
        /// </summary>
        public void SetHoliday(DateTime date)
        {
            /* 
             * - Henter bare ut .Date komponenten av DateTime, som en ekstra sikring for å sørge for at man ikke senere i 
             * koden får følgefeil.
             * 
             * - Man kan argumentere for at man skal kaste exceptions hvis date input har timer, sekunder eller ms satt,
             * men dette mener jeg er for agressive kasting av exceptions da en kan tenke seg at en bruker av denne klassen
             * med uhell sender inn denne infoen i sære tilfeller. I tillegg så er det intuitivt å forstå at denne metoden omhandler
             * hele dager når det ikke er en til-fra metode, og er også dokumenter i metode-summary.
             * 
             */
            Holidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            throw new NotImplementedException();
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            throw new NotImplementedException();
        }

        public List<DateTime> GetHolidays()
        {
            return Holidays;
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
