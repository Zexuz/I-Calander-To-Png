using System;
using System.Collections.Generic;


namespace ICalendarToPng {

    internal class Program {

        private const string CalendarUrl =
            "http://p50-calendars.icloud.com/published/2/o8iZoXrJGPwFY0MtgV6ll7aTw_-g_mFshHjieIDuyz6i_SKYvqTscmcGUSsqlpawDiO2B9qet2Nhot-1_eTide4ZGv64FqjmBQWfd4Xwc9w";

        public static void Main(string[] args) {
            var icalc = new ICalc(CalendarUrl);
            var display = new Display(300, 300, 1);

            // icalc.DownloadFile();
            var file = icalc.ReadFile();

            var formater = new Formater(file);
            formater.MakeEvents();


            var list = formater.GetCalendarEventsBetweenDates(new DateTime(2016, 09,26 ), new DateTime(2016, 10, 01));
            //todo convert list to weeks

            var startWeek = new Week(list[0].Start);

            Console.WriteLine(list[0].Start);

            var days = new List<Day>();


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

                #region comment

                /*


                    int i;
                                 while ((i = lastIndex) < list.Count) {
                                     var index = i;

                                     Console.WriteLine("----------------starting while loop");
                                     while (true) {
                                         //list[i].Start.Day == list[index].Start.Day && index + 1 < list.Count
                                         if (index == list.Count) break;
                                         if (list[i].Start.Day != list[index].Start.Day) break;

                                         Console.WriteLine("list[i] day (static) {0}", list[i].Start.Day);
                                         Console.WriteLine("list[index] day {0}", list[index].Start.Day);
                                         cEvents.Add(list[index]);
                                         index++;
                                     }

                                     lastIndex = index;

                                     Console.WriteLine("Ending while loop-------------------");
                                 }



                 */

                #endregion
            }

            //When we print out our calendar, just take 7 days at a time unitll done and make a image

            //god code but we can't doo this untill all days are in weeks and all calendar events is inside days
            var indexInWeek = (int) list[0].Start.DayOfWeek - 1;
            Console.WriteLine(indexInWeek);

            var calanderGraphics = new CalendarEventGraphicsWraper(display);

            foreach (var day in days) {

                if (day.CalendarEvents == null) {
                    //draw "nothing here" on img
                    continue;
                }

                foreach (var cEvent in day.CalendarEvents) {
                    calanderGraphics.DrawCalanderEvent(cEvent, (int) day.CalendarEvents[0].Start.DayOfWeek);
                }
            }

            display.SaveImage();
        }

    }

}