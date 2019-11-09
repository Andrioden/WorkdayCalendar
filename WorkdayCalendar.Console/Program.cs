using System;

namespace WorkdayCalendar.Console
{
    class Program
    {
        private static IWorkdayCalendar calendar;

        private static void Main(string[] args)
        {
            calendar = new WorkdayCalendar();
            calendar.SetWorkdayStartAndStop(8, 0, 16, 0);
            calendar.SetRecurringHoliday(5, 17);
            calendar.SetHoliday(new DateTime(2004, 5, 27));

            RunTest(new DateTime(2004, 5, 24, 18, 5, 0), -5.5m);
            System.Console.WriteLine("");
            RunTest(new DateTime(2004, 5, 24, 19, 3, 0), 44.723656m);
            RunTest(new DateTime(2004, 5, 24, 18, 3, 0), -6.7470217m);
            RunTest(new DateTime(2004, 5, 24, 08, 3, 0), 12.782709m);
            RunTest(new DateTime(2004, 5, 24, 07, 3, 0), 8.276628m);

            System.Console.WriteLine("");
            System.Console.WriteLine("Trykk en knapp for å lukke konsollapplikasjonen");
            System.Console.ReadKey();
        }

        private static void RunTest(DateTime start, decimal increment)
        {
            string format = "dd-MM-yyyy HH:mm";
            var incrementedDate = calendar.GetWorkdayAfterIncremetedWorkdays(start, increment);

            System.Console.WriteLine(
                start.ToString(format) +
                " med tillegg av " +
                increment +
                " arbeidsdager er " +
                incrementedDate.ToString(format)
            );
        }
    }
}
