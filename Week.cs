using System;
using System.Collections.Generic;
using System.Globalization;

using ICalendarToPng.files;

namespace ICalendarToPng {

    public class Week {

        public List<Day> Days = new List<Day>(new Day[7]);

        public int WeekNr;
        public DateTime FirstDayOfWeek;

        public Week() {
        }

        public Week(DateTime date) {
            SetWeek(date);
        }

        public void SetWeek(DateTime date) {
            WeekNr = GetCurrentWeekNr(date);
            FirstDayOfWeek = date.Subtract(TimeSpan.FromDays(DaysIntoTheWeek(date)));
            WeekNr = GetIso8601WeekOfYear(date);

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

        public static int GetIso8601WeekOfYear(DateTime time) {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                time,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }

    }


    public class Day {

        public List<Event> Events;

        public Day(List<Event> events) {
            Events = events;
        }

    }

}