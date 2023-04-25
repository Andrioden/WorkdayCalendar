using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using WC.Human;

namespace WC.Tests
{
    /*
     * Jeg liker å navngi klasser etter følgende format: [TypeTest]_[Klasse]_[Metode]_[ExtraInfo]
     * [TypeTest] er enten UT for UnitTest eller IT for Integrasjonstest.
     * [Klasse] er navnet på klassen som testes
     * [Metode] er navnet på klassens metode som testes
     * [ExtraInfo] er annet verdt å ha i navnet, for eks hvis metoden som testes er kompleks og kan ha forskjellig input
     * 
     * Intensjonen er å enkelt i alle perspektiver kunne detektere hva som har feilet, om det er i visual studio test eksploreren
     * eller i logger fra eksterne systemer som kjører tester automatisk. Det gjør det også enklere å søke etter tester i test explorer.
     * 
     * I større komplekse prosjekter kan jeg legge [Namespace] eller [Område] før [Klasse] som en ekstra "path"-identifikator.
     */
    [TestClass]
    public class WorkdayCalendarTests
    {

        private const string DATE_FORMAT = "dd-MM-yyyy HH:mm";

        /*
         * Jeg kunne initialisert WorkdayCalendar i TestInitialize. Jeg liker ikke å gjøre dette, for å begrense "magi"
         * som skjer utenfor hver enkelt testmetode. Jeg liker å raskt kunne "se" alt som skjer innad hver enkelt testmetode.
         * 
         * Man kan argumentere for at alle som leser tester bør ha et forhold til TestInitialize eller tilsvarende, men her heller jeg mot,
         * og istedet repetere kode. Hadde opprettelsen av WorkdayCalendar vær kompleks hadde jeg lagt den i en egen opprettelses-hjelpemetode.
         */
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [DataTestMethod]
        [DataRow(00, 00, 00, 00,   0)]
        [DataRow(00, 00, 01, 00,   60)]
        [DataRow(00, 00, 01, 01,   61)]
        [DataRow(23, 00, 00, 01,   61)]
        public void UT_WorkdayCalendar_GetWorkdayDuration(int startHours, int startMinutes, int stopHours, int stopMinutes, int expTotalMinutes)
        {
            WorkdayCalendar workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(startHours, startMinutes, stopHours, stopMinutes);

            Assert.AreEqual(expTotalMinutes, workdayCalendar.GetWorkdayDuration().TotalMinutes);
        }

        [DataTestMethod]
        // Starts and stops on same day
        [DataRow(00, 00, 08, 00, "24-05-2004 00:00",   true)]
        [DataRow(00, 00, 08, 00, "24-05-2004 00:01",   true)]
        [DataRow(00, 00, 08, 00, "24-05-2004 07:59",   true)]
        [DataRow(00, 00, 08, 00, "24-05-2004 08:00",   true)]
        [DataRow(00, 00, 08, 00, "24-05-2004 08:01",   false)]
        [DataRow(00, 00, 08, 00, "24-05-2004 23:59",   false)]
        // Starts and stops crosses midnight
        [DataRow(23, 00, 01, 00, "24-05-2004 23:00",   true)]
        [DataRow(23, 00, 01, 00, "24-05-2004 23:01",   true)]
        [DataRow(23, 00, 01, 00, "24-05-2004 00:59",   true)]
        [DataRow(23, 00, 01, 00, "24-05-2004 01:00",   true)]
        [DataRow(23, 00, 01, 00, "24-05-2004 01:01",   false)]
        [DataRow(23, 00, 01, 00, "24-05-2004 22:59",   false)]
        public void UT_WorkdayCalendar_IsWithinWorkday(int startHours, int startMinutes, int stopHours, int stopMinutes, string dateStr, bool expIsWithinWorkday)
        {
            WorkdayCalendar workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(startHours, startMinutes, stopHours, stopMinutes);

            DateTime date = DateTime.ParseExact(dateStr, DATE_FORMAT, CultureInfo.InvariantCulture);
            Assert.AreEqual(expIsWithinWorkday, workdayCalendar.IsWithinWorkday(date));
        }

        [DataTestMethod]
        // Forwards
        [DataRow(00, 00, 08, 00, "24-05-2004 00:00", 1.0,   "24-05-2004 08:00")]
        [DataRow(00, 00, 08, 00, "24-05-2004 00:00", 2.0,   "25-05-2004 08:00")]
        // Backwards
        [DataRow(00, 00, 08, 00, "24-05-2004 08:00", -1.0,   "24-05-2004 00:00")]
        [DataRow(00, 00, 08, 00, "25-05-2004 08:00", -2.0,   "24-05-2004 00:00")]
        public void UT_WorkdayCalendar_GetWorkdayAfterIncremetedWorkdays(
            int startHours,
            int startMinutes,
            int stopHours,
            int stopMinutes,
            string startDateStr,
            double incrementWorkDays, // DataRow attribute dont accept decimals
            string expWorkdayStr
        )
        {
            WorkdayCalendar workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(startHours, startMinutes, stopHours, stopMinutes);

            DateTime workday = workdayCalendar.GetWorkdayAfterIncremetedWorkdays(
                DateTime.ParseExact(startDateStr, DATE_FORMAT, CultureInfo.InvariantCulture),
                (decimal)incrementWorkDays
            );

            Assert.AreEqual(expWorkdayStr, workday.ToString(DATE_FORMAT));
        }
    }
}