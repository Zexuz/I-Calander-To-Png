using System;
using System.Collections.Generic;

namespace ICalanderToPang {

    internal class Program {

        private const string CalanderUrl =
            "http://p50-calendars.icloud.com/published/2/o8iZoXrJGPwFY0MtgV6ll7aTw_-g_mFshHjieIDuyz6i_SKYvqTscmcGUSsqlpawDiO2B9qet2Nhot-1_eTide4ZGv64FqjmBQWfd4Xwc9w";

        public static void Main(string[] args) {
            var icalc = new ICalc(CalanderUrl);

            //    icalc.DownloadFile();
            var file = icalc.ReadFile();

            Formater.GetEvents(file);

            Console.ReadKey();

        }


    }

}