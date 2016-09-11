using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ICalendarToPng {

    public class Week {

        public List<Day> Days;
        public int WeekNr;
        public DateTime FirstDayOfWeek;

        public Week() {
            WeekNr = GetCurrentWeekNr();
            FirstDayOfWeek = DateTime.Today.Subtract(TimeSpan.FromDays(DaysIntoTheWeek()));
        }

        private static int GetCurrentWeekNr() {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var cal = dfi.Calendar;

            return cal.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        private static int DaysIntoTheWeek() {
            var date = DateTime.Today;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (date.DayOfWeek) {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                default:
                    return 6;
            }
        }

        public bool WeekDayIsInsideWeek(DateTime date) {
            TimeSpan res = FirstDayOfWeek.Date - date.Date;

            return res.Days > -7 && res.Days <= 0;
        }

    }


    public class Day {

        public List<CalendarEvent> CalendarEvents;

        public Day(List<CalendarEvent> calendarEvents) {
            CalendarEvents = calendarEvents;
        }

    }

}