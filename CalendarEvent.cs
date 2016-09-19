using System;

namespace ICalendarToPng {

    public class CalendarEvent {

        public DateTime Start;
        public DateTime End;

        public string Location;
        public string Subject;

        public string Uid;

        public Repeat Repeat;


        public override string ToString() {
            return "Start: " + Start + ", " + "End: " + End + ", " + "Location: " + Location + ", " + "Subject: " +
                   Subject;
        }

    }

    public class Repeat {

        public FreqEnum Freq = FreqEnum.Unknow;
        public DateTime Untill;

        public Repeat(string longAsNoneIndexedString) {
            try {
                var repeatProp = Formater.GetIcalcProp("RRULE:", longAsNoneIndexedString);
                var freqStartIndex = repeatProp.IndexOf("FREQ=") + 5;
                var freqEndIndex = repeatProp.IndexOf(";");
                var freq = repeatProp.Substring(freqStartIndex, freqEndIndex - freqStartIndex);

                var untillStartIndex = repeatProp.IndexOf("UNTIL=") + 6;
                var untillEndIndex = repeatProp.IndexOf("Z");
                var untillStr = repeatProp.Substring(untillStartIndex, untillEndIndex - untillStartIndex);
                Console.WriteLine(freq);
                Console.WriteLine(untillStr);

                Untill = Formater.GetDateTimeFromString(untillStr);
                SetFreqWithString(freq);
            }
            catch (Exception e) {
                Console.WriteLine("Event is none reapeted and would break if it where no for this!");
            }
        }

        public Repeat(DateTime untill, string freq) {
            Untill = untill;
            SetFreqWithString(freq);
        }

        public void SetFreqWithString(string freq) {
            switch (freq) {
                case "DAILY":
                    Freq = FreqEnum.Daily;
                    break;
                case "WEEKLY":
                    Freq = FreqEnum.Weekly;
                    break;
                case "MONTHLY":
                    Freq = FreqEnum.Monthly;
                    break;
                case "YEARLY":
                    Freq = FreqEnum.Yearly;
                    break;
                default:
                    Freq = FreqEnum.Unknow;
                    break;
            }
        }

        public enum FreqEnum {

            Daily = 1,
            Weekly = 2,
            Monthly = 3,
            Yearly = 4,
            Unknow = 5

        }

    }

}