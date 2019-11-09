using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WorkdayCalendar.Tests
{
    /*
     * Jeg liker � navngi klasser etter f�lgende format: [TypeTest]_[Klasse]_[Metode]_[ExtraInfo]
     * [TypeTest] er enten UT for UnitTest eller IT for Integrasjonstest.
     * [Klasse] er navnet p� klassen som testes
     * [Metode] er navnet p� klassens metode som testes
     * [ExtraInfo] er annet verdt � ha i navnet, for eks hvis metoden som testes er kompleks og kan ha forskjellig input
     * 
     * Intensjonen er � enkelt i alle perspektiver kunne detektere hva som har feilet, om det er i visual studio test eksploreren
     * eller i logger fra eksterne systemer som kj�rer tester automatisk. Det gj�r det ogs� enklere � s�ke etter tester i test explorer.
     * 
     * I st�rre komplekse prosjekter kan jeg legge [Namespace] eller [Omr�de] f�r [Klasse] som en ekstra "path"-identifikator.
     */
    [TestClass]
    public class WorkdayCalendarTests
    {
        [TestMethod]
        public void UT_WorkdayCalendar_SetHoliday()
        {
            WorkdayCalendar workdayCalendar = new WorkdayCalendar();

            workdayCalendar.SetHoliday(new DateTime(2004, 1, 1));
            Assert.AreEqual(1, workdayCalendar.GetHolidays().Count);
            Assert.AreEqual(workdayCalendar.GetHolidays()[0], new DateTime(2004, 1, 1));

            workdayCalendar.SetHoliday(new DateTime(2004, 1, 2));
            Assert.AreEqual(2, workdayCalendar.GetHolidays().Count);
        }
    }
}