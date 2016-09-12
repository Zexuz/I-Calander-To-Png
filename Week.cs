using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ICalendarToPng {

    public class Week {

        public List<Day> Days;

        public int WeekNr;
        public DateTime FirstDayOfWeek;

        public Week(DateTime date) {
            WeekNr = GetCurrentWeekNr(date);
            FirstDayOfWeek = date.Subtract(TimeSpan.FromDays(DaysIntoTheWeek(date)));

            Days = new List<Day>(7);
        }

        private static int GetCurrentWeekNr(DateTime date) {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var cal = dfi.Calendar;

            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static int DaysIntoTheWeek(DateTime date) {
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