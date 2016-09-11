using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ICalendarToPng {

    public class ICalc {

        private readonly string _iCalcUrl;


        public ICalc(string iCalcUrl) {
            _iCalcUrl = iCalcUrl;
        }


        public void DownloadFile() {
            using (var client = new WebClient()) {
                Console.WriteLine("Downloading file from {0}", _iCalcUrl);
                client.DownloadFile(_iCalcUrl, @"..\..\res\ithsSchema.ics");
            }
        }

        public string ReadFile() {
            string file;

            using (var sr = new StreamReader(@"..\..\res\ithsSchema.ics")) {
                file = sr.ReadToEnd();
            }

            return file;
        }

    }

    public class Formater {

        public static List<CalendarEvent> GetEvents(string iCalcFile) {
            var list = new List<CalendarEvent>();

            var startIndex = 0;
            while ((startIndex = GetNextEventStartPos(iCalcFile, startIndex + 1)) > -1) {
                var file = iCalcFile.Substring(startIndex);

                var calendarEvent = new CalendarEvent();

                calendarEvent.Subject = GetIcalcProp("SUMMARY:", file);
                calendarEvent.Location = GetIcalcProp("LOCATION:", file);
                calendarEvent.Start = GetDateTimeFromString(GetIcalcProp("DTSTART;", file));
                calendarEvent.End = GetDateTimeFromString(GetIcalcProp("DTEND;", file));

                list.Add(calendarEvent);
                Console.WriteLine(calendarEvent.ToString());
            }

            /*

            var calanderEvent = new CalendarEvent();

            calanderEvent.Subject = rows[++index];
            calanderEvent.Location = rows[++index];
            calanderEvent.Start = rows[++index];
            calanderEvent.End = rows[++index];

            */

            return list;
        }

        public static string GetIcalcProp(string propName, string file) {
            var index = file.IndexOf(propName);
            var sIndex = index + propName.Length;
            var eIndex = file.IndexOf("\r\n", index);
            var lenght = eIndex - sIndex;

            var returnMsg = file.Substring(sIndex, lenght);

            if (propName != "LOCATION:")
                return returnMsg; //todo bad AKA HORRIABLE way of cheking if the property is set or not...

            if (returnMsg.Length == 0) returnMsg = "Unknown Location";

            return returnMsg;
        }

        public static int GetNextEventStartPos(string str, int startIndex = 0) {
            return str.IndexOf("BEGIN:VEVENT", startIndex);
        }

        public static DateTime GetDateTimeFromString(string str) {
            str = str.Substring(str.IndexOf(':')+1);
            var year = int.Parse(str.Substring(0, 4));
            var month = int.Parse(str.Substring(4, 2));
            var day = int.Parse(str.Substring(6, 2));
            var hh = int.Parse(str.Substring(9, 2));
            var mm = int.Parse(str.Substring(11, 2));
            var ss = int.Parse(str.Substring(13, 2));

            return new DateTime(year, month, day, hh, mm, ss);
        }

    }


    public class CalendarEvent {

        public DateTime Start;
        public DateTime End;

        public string Location;
        public string Subject;


        public override string ToString() {
            return "Start: " + Start + ", " + "End: " + End + ", " + "Location: " + Location + ", " + "Subject: " +
                   Subject;
        }

    }

}