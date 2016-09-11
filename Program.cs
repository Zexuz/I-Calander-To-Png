using System;

namespace ICalendarToPng {

    internal class Program {

        private const string CalendarUrl =
            "http://p50-calendars.icloud.com/published/2/o8iZoXrJGPwFY0MtgV6ll7aTw_-g_mFshHjieIDuyz6i_SKYvqTscmcGUSsqlpawDiO2B9qet2Nhot-1_eTide4ZGv64FqjmBQWfd4Xwc9w";

        public static void Main(string[] args) {
            var icalc = new ICalc(CalendarUrl);

            var calanderGraphics = new CalendarEventGraphicsWraper(300,300,1);



             // icalc.DownloadFile();
            var file = icalc.ReadFile();
            Formater.GetEvents(file);

            calanderGraphics.DrawCalanderEvent(Formater.GetEvents(file)[0]);
            calanderGraphics.DrawCalanderEvent(Formater.GetEvents(file)[4]);
            calanderGraphics.SaveImage();


        }


    }

}