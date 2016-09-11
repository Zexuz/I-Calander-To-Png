using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Policy;

namespace ICalanderToPang {

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

        public static List<CalanderEvent> GetEvents(string iCalcFile) {
            var list = new List<CalanderEvent>();

            var startIndex = 0;
            while ((startIndex = GetNextEventStartPos(iCalcFile, startIndex + 1)) > -1) {
                var file = iCalcFile.Substring(startIndex);

                var calanderEvent = new CalanderEvent();

                calanderEvent.Subject = GetIcalcProp("SUMMARY:", file);
                calanderEvent.Location = GetIcalcProp("LOCATION:", file);
                calanderEvent.Start = GetIcalcProp("DTSTART;", file);
                calanderEvent.End = GetIcalcProp("DTEND;", file);

                list.Add(calanderEvent);
                Console.WriteLine(calanderEvent.ToString());
            }

            /*

            var calanderEvent = new CalanderEvent();

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
            var eIndex = file.IndexOf("\n", index);
            var lenght = eIndex - sIndex;

            return file.Substring(sIndex, lenght);
        }

        public static int GetNextEventStartPos(string str, int startIndex = 0) {
            return str.IndexOf("BEGIN:VEVENT", startIndex);
        }

    }


    public class CalanderEvent {

        public string Start;
        public string End;

        public string Location;
        public string Subject;


        public override string ToString() {
            return "Start: " + Start + ", " + "End: " + End + ", " + "Location: " + Location + ", " + "Subject: " +
                   Subject;
        }

    }

}