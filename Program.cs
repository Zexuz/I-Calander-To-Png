using System;

using Microsoft.SqlServer.Server;

namespace ICalendarToPng {

    internal class Program {

        private const string CalendarUrl =
            "http://p50-calendars.icloud.com/published/2/o8iZoXrJGPwFY0MtgV6ll7aTw_-g_mFshHjieIDuyz6i_SKYvqTscmcGUSsqlpawDiO2B9qet2Nhot-1_eTide4ZGv64FqjmBQWfd4Xwc9w";

        public static void Main(string[] args) {

            Environment.Exit(Environment.ExitCode);

            var icalc = new ICalc(CalendarUrl);

             // icalc.DownloadFile();
            var file = icalc.ReadFile();

            var formater = new Formater(file);
            formater.MakeEvents();


            var calanderGraphics = new CalendarEventGraphicsWraper(300, 300, 1);
            calanderGraphics.DrawCalanderEvent(formater.List[0]);
            calanderGraphics.DrawCalanderEvent(formater.List[1]);
            calanderGraphics.DrawCalanderEvent(formater.List[3]);
            calanderGraphics.SaveImage();


            //todo sort calanderEvent based on time start

        }


    }

}