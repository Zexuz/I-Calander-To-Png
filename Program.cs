using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

using ICalendarToPng.files;


namespace ICalendarToPng {

    internal class Program {

        private const string CalendarUrl =
            "http://p50-calendars.icloud.com/published/2/o8iZoXrJGPwFY0MtgV6ll7aTw_-g_mFshHjieIDuyz6i_SKYvqTscmcGUSsqlpawDiO2B9qet2Nhot-1_eTide4ZGv64FqjmBQWfd4Xwc9w";

        public static void Main(string[] args) {
            var icalc = new ICalc(CalendarUrl);

            icalc.DownloadFile();
            var file = icalc.ReadFile();

            var formater = new Formater(file);


            //make events from IcalsEvents;
            formater.MakeEventsFromIcalcEvents(new DateTime(2016, 09, 19), new DateTime(2016, 10, 1));

            formater.SortList();

            var list = formater.GetCalendarEventsBetweenDates(new DateTime(2016, 09, 19), new DateTime(2016, 10, 1));

            Console.WriteLine(list.Count);

            var days = new List<Day>();

            //todo find a place for this code

            #region GetAndAddDaysToList

            var currentcEventIndex = 0;
            for (var date = list[0].Start; date <= list[list.Count - 1].End; date = date.AddDays(1)) {
                //get the fisrt day in our calendar
                //get the last day in our calendar
                //get a calendar and run trhue every day

                var events = new List<Event>();

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

                    events.Add(list[i]);
                }

                currentcEventIndex += events.Count;
                var day = new Day(events);
                days.Add(day);
            }

            #endregion

            //When we print out our calendar, just take 7 days at a time unitll done and make a image

            //god code but we can't doo this untill all days are in weeks and all calendar events is inside days

            Console.WriteLine($"days {days.Count}");

            var weeks = new List<Week>();

            Week week = null;
            for (var i = 0; i < days.Count; i++) {
                var day = days[i];

                if (day == null || day.Events == null) continue;

                if (week == null) week = new Week(days[i].Events[0].Start);

                if (i % 7 == 0 && i > 0) {
                    weeks.Add(week);
                    week = new Week(); //days[i].CalendarEvents[0].Start
                    //todo this will give us a empty week whith no start day eg 1970-01-01 00:00:00 if the week is empty
                }

                Console.WriteLine($"index is {i}");

                if (!week.WeekDayIsInsideWeek(day.Events[0].Start)) {
                    weeks.Add(week);
                    week = new Week(day.Events[0].Start);
                }

                var index = (int) day.Events[0].Start.DayOfWeek - 1;
                week.Days.RemoveAt(index);
                week.Days.Insert(index, day);
            }

            weeks.Add(week);

            Console.WriteLine($"We have {weeks.Count} st weeks");
            Console.WriteLine(list.Count);

            foreach (var w in weeks) {
                new Image(800, 800, 50).WriteWeek(w);
            }

            Console.WriteLine("Done");
        }

        static void exit() {
            Environment.Exit(Environment.ExitCode);
        }

    }

}