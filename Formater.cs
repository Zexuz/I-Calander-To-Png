using System;
using System.Collections.Generic;
using System.Linq;

namespace ICalendarToPng {

    public class Formater {

        public List<CalendarEvent> List;
        public string ICalcFile;

        public Formater(string iCalcFile) {
            ICalcFile = iCalcFile;
            List = new List<CalendarEvent>();


            List = MakeEvents();
        }

        public List<CalendarEvent> MakeEvents() {
            var startIndex = 0;


            while ((startIndex = GetNextEventStartPos(ICalcFile, startIndex + 1)) > -1) {
                var file = ICalcFile.Substring(startIndex);

                var calendarEvent = new CalendarEvent();

                calendarEvent.Subject = GetIcalcProp("SUMMARY:", file);
                calendarEvent.Location = GetIcalcProp("LOCATION:", file);
                calendarEvent.Start = GetDateTimeFromString(GetIcalcProp("DTSTART;", file));
                calendarEvent.End = GetDateTimeFromString(GetIcalcProp("DTEND;", file));

                List.Add(calendarEvent);
            }

            SortList();
            return List;
        }

        public List<CalendarEvent> GetCalendarEventsBetweenDates(DateTime date1, DateTime date2) {
            return List.Where(calendarEvent => calendarEvent.Start >= date1 && calendarEvent.End <= date2).ToList();
        }

        private void SortList() {
            List = List.OrderBy(o => o.Start).ToList();
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
            str = str.Substring(str.IndexOf(':') + 1);
            var year = int.Parse(str.Substring(0, 4));
            var month = int.Parse(str.Substring(4, 2));
            var day = int.Parse(str.Substring(6, 2));
            var hh = int.Parse(str.Substring(9, 2));
            var mm = int.Parse(str.Substring(11, 2));
            var ss = int.Parse(str.Substring(13, 2));

            return new DateTime(year, month, day, hh, mm, ss);
        }

    }

}