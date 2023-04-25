# Input

```
Arbeidsdager-kalkulator (.Net-versjon) 
Vi trenger en kalkulator for arbeidsdager: 
WorkdayCalendar workdayCalendar = new WorkdayCalendar(); 
Kalenderen skal kunne regne på avstand i arbeidsdager fra en gitt dato (DateTime). En 
arbeidsdag er en dag fra mandag til fredag som ikke er en helligdag. WorkdayCalendar har 2 
metoder for å fortelle om hvilke dager skal betraktes som helligdager: 
workdayCalendar.SetHoliday(DateTime date) 
sier at den gitte dato er en helligdag og skal ikke betraktes som arbeidsdag. 
workdayCalendar.SetRecurringHoliday(int month, int day) 
sier at den gitte dag/måned er å betrakte som helligdag hvert år. 
workdayCalendar.SetWorkdayStartAndStop(int startHours, int startMinutes, 
int stopHours, int stopMinutes) 
setter start og stopp klokkeslett for arbeidsdagen, f.eks. 8:00 - 16:00: 
workdayCalendar.SetWorkdayStartAndStop(8, 0, 16, 0); 
Kjernen i løsningen blir følgende metode: 
public DateTime GetWorkdayIncrement(Date startDate, decimal 
incrementInWorkdays) 
Metoden må alltid returnere et klokkeslett mellom de 2 punktene definert i kallet 
SetWorkdayStartAndStop, selv om startDate ikke behøver å følge regelen selv. På denne 
måten blir kl 15:07 + 0,25 arbeidsdager kl 9:07, og kl 04:00 pluss 0,5 arbeidsdager lik kl 12:00. 
Klassen skal med andre ord implementere følgende interface: 
namespace WorkdayNet  
{ 
    public interface IWorkdayCalendar 
    { 
        void SetHoliday(DateTime date); 
        void SetRecurringHoliday(int month, int day); 
        void SetWorkdayStartAndStop(int startHours, int startMinutes, 
int stopHours, int stopMinutes); 
        DateTime GetWorkdayIncrement(DateTime startDate, decimal 
incrementInWorkdays); 
        string GetName(); 
    } 
} 
Klassen kan testes med kode som denne: 
!
IWorkdayCalendar calendar = new WorkdayCalendar(); 
calendar.SetWorkdayStartAndStop(8, 0, 16, 0); 
calendar.SetRecurringHoliday(5, 17); 
calendar.SetHoliday(new DateTime(2004, 5, 27)); 
string format = "dd-MM-yyyy HH:mm"; 
var start = new DateTime(2004, 5, 24, 18, 5, 0); 
decimal increment = -5.5m; 
var incrementedDate = calendar.GetWorkdayIncrement(start, increment); 
Console.WriteLine( 
start.ToString(format) + 
" med tillegg av " + 
increment + 
" arbeidsdager er " + 
incrementedDate.ToString(format)); 
Noe som burde gi følgende resultat: 
24-05-2004 18:05 med tillegg av -5.5 arbeidsdager er 14-05-2004 12:00 
Andre korrekte resultater: 
24-05-2004 19:03 med tillegg av 44.723656 arbeidsdager er 27-07-2004 13:47
24-05-2004 18:03 med tillegg av -6.7470217 arbeidsdager er 13-05-2004 10:02
24-05-2004 08:03 med tillegg av 12.782709 arbeidsdager er 10-06-2004 14:18
24-05-2004 07:03 med tillegg av 8.276628 arbeidsdager er 04-06-2004 10:12 
Hele saken bør løses i én klassefil, evt. med indre klasser hvis du trenger dem. De beste 
løsninger er lett å forstå, relativt korte (under 250 java-linjer), og bør selvsagt komme frem til 
riktig svar på en rekke forskjellige tilfeller.  
Lykke til!
```

# Output



Her er en mulig implementasjon av WorkdayCalendar i C#:

```csharp
using System;
using System.Collections.Generic;

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
```

Noen notater om implementasjonen:

- Jeg brukte en HashSet for å lagre helligdagene og gjentatte helligdager, for enkel og rask søkning.
- Jeg brukte TimeSpan for å representere start- og stopptiden for arbeidsdagen.
- I GetWork