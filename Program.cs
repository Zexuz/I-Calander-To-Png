using System;
using System.Collections.Generic;


namespace ICalendarToPng {

    internal class Program {

        private const string CalendarUrl =
            "http://p50-calendars.icloud.com/published/2/o8iZoXrJGPwFY0MtgV6ll7aTw_-g_mFshHjieIDuyz6i_SKYvqTscmcGUSsqlpawDiO2B9qet2Nhot-1_eTide4ZGv64FqjmBQWfd4Xwc9w";

        public static void Main(string[] args) {
            var icalc = new ICalc(CalendarUrl);
            var display = new Display(800, 800, 1);

            //icalc.DownloadFile();
            var file = icalc.ReadFile();

            var formater = new Formater(file);
            formater.MakeEvents();


            var list = formater.GetCalendarEventsBetweenDates(new DateTime(2016, 09, 19), new DateTime(2016, 09, 24));
            //todo convert list to weeks


            var days = new List<Day>();

            //todo find a place for this code
            var currentcEventIndex = 0;
            for (var date = list[0].Start; date <= list[list.Count - 1].End; date = date.AddDays(1)) {
                //get the fisrt day in our calendar
                //get the last day in our calendar
                //get a calendar and run trhue every day

                var cEvents = new List<CalendarEvent>();

                //if there are no "events" in that day, just add a empty day
                if (date.Day != list[currentcEventIndex].Start.Day) {
                    days.Add(new Day(null));
                    continue;
                }

                //Now we check our dayes in our list agains the current day (the day we are @ in our iteration)
                //And add all the events to this day.
                for (var i = currentcEventIndex; i < list.Count; i++) {
                    if (date.Day != list[i].Start.Day) {
                        break;
                    }

                    cEvents.Add(list[i]);
                }

                currentcEventIndex += cEvents.Count;
                var day = new Day(cEvents);
                days.Add(day);
            }

            //When we print out our calendar, just take 7 days at a time unitll done and make a image

            //god code but we can't doo this untill all days are in weeks and all calendar events is inside days

            display.PaintDays(days);

            display.SaveImage();
        }

    }

}