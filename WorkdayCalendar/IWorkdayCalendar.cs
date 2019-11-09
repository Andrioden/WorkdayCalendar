using System;
using System.Collections.Generic;
using System.Text;

namespace WorkdayCalendar
{
    /*
     * Jeg har gjort 2 endringer på interfacet som jeg mente var fornuftig. Dette følte jeg at jeg kunne gjøre da
     * jeg har full kontroll på bruken av interfacet. Men hvis interfacet har andre avhengigheter så ville jeg nølt med
     * å endre det hvis jeg ikke har kontroll på alle andre avhengigheter.
     */
    public interface IWorkdayCalendar
    {
        void SetHoliday(DateTime date);
        void SetRecurringHoliday(int month, int day);
        void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes);

        /*
         * ENDRING #1
         * 
         * Jeg har valgt å rename denne metoden til noe mere forståelig. GetWorkdayIncrement høres for meg ut som a skal
         * returnere en increment/periode/tidsløp som kreves for x input.
         * 
         * Alternativ navn hadde vært GetWorkdayByIncrement, GetWorkdayAfterIncrement, ol.
         */
        DateTime GetWorkdayAfterIncremetedWorkdays(DateTime startDate, decimal incrementInWorkdays);

        /*
         * ENDRING #2
         * 
         * Jeg har også valgt å kommentere ut denne interface metoden, normalt sett ville jeg også ha fjernet den.
         */
        //string GetName();
    }
}
