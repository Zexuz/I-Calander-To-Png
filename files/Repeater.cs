using System;

namespace ICalendarToPng.files {

    public class Repeater {

        public FreqEnum Freq { get; set; }
        public DateTime Untill { get; set; }


        public enum FreqEnum {

            DAILY,
            WEEKLY,
            MONTHLY,
            YEARLY,
            UNKNOW

        }

    }

}