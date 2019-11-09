/*
 * Du vil se at det er summary kommentarer på engelsk, dette er kommentarer til den teoretiske utvikleren som bruker denne klassen.
 * 
 * De norske kommentarene i metodene er mine kommentarer ifb Vivende code review.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkdayCalendar
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        private List<DateTime> Holidays = new List<DateTime>();
        private List<(int Month, int Day)> RecurringHolidays = new List<(int Month, int Day)>();

        private int StartHours;
        private int StartMinutes;
        private int StopHours;
        private int StopMinutes;

        private readonly List<DayOfWeek> WeekendDays = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };

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
             */
            Holidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            RecurringHolidays.Add((Month: month, Day: day));
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            StartHours = startHours;
            StartMinutes = startMinutes;
            StopHours = stopHours;
            StopMinutes = stopMinutes;
        }

        public TimeSpan GetWorkdayDuration()
        {
            if (IsWorkdayStartAndStopOnSameDay())
                return new DateTime(2000, 1, 1, StopHours, StopMinutes, 0) - new DateTime(2000, 1, 1, StartHours, StartMinutes, 0);
            else // Arbeidsdagen krysser natten
                return new DateTime(2000, 1, 2, StopHours, StopMinutes, 0) - new DateTime(2000, 1, 1, StartHours, StartMinutes, 0);
        }

        public bool IsWithinWorkday(DateTime d)
        {
            if (IsWorkdayStartAndStopOnSameDay())
                return IsBetweenStartAndStop(d);
            else // Arbeidstid krysser natten
            {
                if (IsBetweenStartAndStop(d, -1, 0))
                    return true;
                else
                    return IsBetweenStartAndStop(d, 0, 1);
            }
        }

        public List<DateTime> GetHolidays()
        {
            return Holidays;
        }

        /// <summary>
        /// Get the next workday after the startDate incremeted by given amount of workdays have lapsed. Precision to minutes.
        /// </summary>
        public DateTime GetWorkdayAfterIncremetedWorkdays(DateTime startDate, decimal incrementInWorkdays)
        {
            /*
             * - Konverterer TotalMinutes til decimal da det er en tryggere konversion
             * - Runder til int da presisjonen til programmet begrenses til minutter
             */
            int totalWorkdaySeconds = (int)Math.Round(Convert.ToDecimal(GetWorkdayDuration().TotalSeconds) * incrementInWorkdays);
            int elapsedWorkdaySeconds = 0;

            // Simuler tiden bakover/fremover til vi har passert nok arbeidsminutter
            int scanIncremention = incrementInWorkdays < 0 ? -1 : 1;
            DateTime scanDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, 0);

            while (elapsedWorkdaySeconds != totalWorkdaySeconds)
            {
                scanDate = scanDate.AddSeconds(scanIncremention);

                if (WeekendDays.Contains(scanDate.DayOfWeek))
                    continue;
                if (Holidays.Any(h => h.Date == scanDate.Date))
                    continue;
                if (RecurringHolidays.Any(h => h.Month == scanDate.Month && h.Day == scanDate.Day))
                    continue;

                /*
                 * Siden vi for vær while loop egentlig vurderer en 1-minutts-periode, så må vi sjekk
                 * både minuttet vi er på, og minuttet vi kommer fra. På den måten vurderer man at hele minuttet er innenfor.
                 */ 
                if (!IsWithinWorkday(scanDate) || !IsWithinWorkday(scanDate.AddSeconds(-scanIncremention)))
                    continue;

                elapsedWorkdaySeconds += scanIncremention;
            }

            return scanDate;
        }

        private bool IsWorkdayStartAndStopOnSameDay()
        {
            return StartHours < StopHours || (StartHours == StopHours && StartMinutes <= StopMinutes);
        }

        private bool IsBetweenStartAndStop(DateTime d, int startDelta = 0, int stopDelta = 0)
        {
            DateTime start = new DateTime(d.Year, d.Month, d.Day, StartHours, StartMinutes, 0).AddDays(startDelta);
            DateTime end = new DateTime(d.Year, d.Month, d.Day, StopHours, StopMinutes, 0).AddDays(stopDelta);
            return start <= d && d <= end;
        }
    }
}