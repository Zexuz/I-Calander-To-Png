using System;
using System.Collections.Generic;
using System.Linq;

using ICalendarToPng.files;

namespace ICalendarToPng {

    public class Formater {

        public List<IcalcEvent> IcalcEventList;
        public string ICalcFile;

        public Formater(string iCalcFile) {
            ICalcFile = iCalcFile;

            IcalcEventList = MakeEvents();
            SortList();
        }

        public List<IcalcEvent> MakeEvents() {
            var startIndex = 0;

            var list = new List<IcalcEvent>();

            while ((startIndex = GetNextEventStartPos(ICalcFile, startIndex + 1)) > -1) {
                var file = ICalcFile.Substring(startIndex);

                var icalcEvent = new IcalcEvent(GetIcalcProp("UID:", file));


                icalcEvent.Subject = GetIcalcProp("SUMMARY:", file);
                icalcEvent.Location = GetIcalcProp("LOCATION:", file);
                icalcEvent.Start = GetDateTimeFromString(GetIcalcProp("DTSTART;", file));
                icalcEvent.End = GetDateTimeFromString(GetIcalcProp("DTEND;", file));



                if (DoesPropExistInEvent("RRULE:", file)) {
                    var RRULE = GetIcalcProp("RRULE:", file);

                    if (RRULE.Length > 0) {
                        var freqString = GetIcalcSubProp("FREQ", RRULE, ";");
                        var untillString = GetIcalcSubProp("UNTIL", RRULE, "Z");


                        Repeater.FreqEnum freqEnum;
                        if (Enum.TryParse(freqString, out freqEnum)) {
                            icalcEvent.Repeater = new Repeater();
                            icalcEvent.Repeater.Freq = freqEnum;
                            icalcEvent.Repeater.Untill = GetDateTimeFromString(untillString);
                        }
                    }
                }
                else {
                    icalcEvent.Repeater = null;
                }


                list.Add(icalcEvent);
            }

            return list;
        }

        public bool DoesPropExistInEvent(string propName, string file) {
            var endIndex = file.IndexOf("END:VEVENT");

            return file.IndexOf(propName) <= endIndex && file.IndexOf(propName) > -1;
        }

        public List<IcalcEvent> GetCalendarEventsBetweenDates(DateTime date1, DateTime date2) {
            return
                IcalcEventList.Where(calendarEvent => calendarEvent.Start >= date1 && calendarEvent.End <= date2)
                    .ToList();
        }

        public void SortList() {
            IcalcEventList = IcalcEventList.OrderBy(o => o.Start).ToList();
        }

        public static string GetIcalcProp(string propName, string file) {
            var propValue = GetIcalcValue(propName, file);

            if (propValue.Length == 0 && propName == "LOCATION:") propValue = "Unknown Location";

            return propValue;
        }

        public static string GetIcalcSubProp(string propName, string file, string endOn) {
            return GetIcalcValue(propName + "=", file, endOn);
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


        private static string GetIcalcValue(string startOnThisSting, string file, string endOnThisString = "\r\n") {
            var index = file.IndexOf(startOnThisSting);
            var sIndex = index + startOnThisSting.Length;
            var eIndex = file.IndexOf(endOnThisString, index);

            var lenght = eIndex - sIndex;

            return file.Substring(sIndex, lenght);
        }

        public List<Event> MakeEventsFromIcalcEvents(DateTime start, DateTime end) {
            var events = new List<Event>();

            foreach (var iEvent in IcalcEventList) {
                events.Add(iEvent.toEvent());

                if (iEvent.Repeater == null) continue;
                if (iEvent.Repeater.Untill < start) continue;


                var numberOfReapeats = GetHowManyRepeatedEventsUntill(start, end, iEvent.Repeater.Freq);

                if (numberOfReapeats == 0) continue;

                for (var i = 0; i < numberOfReapeats; i++) {
                    if (iEvent.Repeater.Freq == Repeater.FreqEnum.DAILY) {
                        iEvent.Start = iEvent.Start.AddDays(1);
                        iEvent.End = iEvent.End.AddDays(1);

                        events.Add(iEvent.toEvent());
                    }

                    if (iEvent.Repeater.Freq == Repeater.FreqEnum.WEEKLY) {
                        iEvent.Start = iEvent.Start.AddDays(7);
                        iEvent.End = iEvent.End.AddDays(7);

                        events.Add(iEvent.toEvent());
                    }

                    if (iEvent.Repeater.Freq == Repeater.FreqEnum.MONTHLY) {
                        iEvent.Start = iEvent.Start.AddMonths(1);
                        iEvent.End = iEvent.End.AddMonths(1);

                        events.Add(iEvent.toEvent());
                    }

                    if (iEvent.Repeater.Freq == Repeater.FreqEnum.YEARLY) {
                        iEvent.Start = iEvent.Start.AddYears(1);
                        iEvent.End = iEvent.End.AddYears(1);

                        events.Add(iEvent.toEvent());
                    }
                }

                // if(iEvent.Repeater.Freq)
                //todo make function that calculates how many repeated events ther will be from now and iEvent.Repeater.Untill.
                //put that number in a loop and add everyevent when loop has done one pass.
            }

            Console.WriteLine("hello wordl!!!");
            return events;
        }

        public static int GetHowManyRepeatedEventsUntill(DateTime start, DateTime end, Repeater.FreqEnum freqEnum) {
            switch (freqEnum) {
                case Repeater.FreqEnum.DAILY:
                    return (int) (end - start).TotalDays;
                case Repeater.FreqEnum.WEEKLY:
                    return (int) (end - start).TotalDays / 7;
                case Repeater.FreqEnum.MONTHLY:
                    return ((end.Year - start.Year) * 12) + end.Month - start.Month;
                case Repeater.FreqEnum.YEARLY:
                    return end.Year - start.Year;
                case Repeater.FreqEnum.UNKNOW:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }

}