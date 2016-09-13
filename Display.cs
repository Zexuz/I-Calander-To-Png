using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ICalendarToPng {

    public class Display {

        private const string OutPutDir = @"../../res/image.png";

        public Bitmap Image { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Scale { get; private set; }


        public Display(int height, int width, int scale) {
            Width = width;
            Height = height;
            Scale = scale;

            Image = new Bitmap(Width, Height);

            ClearAndPaintHourLines();
            PaintDayLines();
        }

        public void PaintDayLines() {}

        public void ClearAndPaintHourLines() {
            using (var g = Graphics.FromImage(Image)) {
                g.Clear(Color.White);

                var semiTransPen = new Pen(Color.FromArgb(64, 0, 0, 255), 1);

                for (int i = 0; i < 24; i++) {
                    var yCord = GetResponsiveValue(i, 24, Height);
                    g.DrawLine(semiTransPen, 0, yCord, Width, yCord);
                }
            }
        }

        public void PaintDays(List<Day> days) {
            foreach (var day in days) {
                if (day.CalendarEvents == null) {
                    //TODO draw "nothing here" on img
                    continue;
                }


                foreach (var cEvent in day.CalendarEvents) {
                    var dayOfWeekk = (int) day.CalendarEvents[0].Start.DayOfWeek;
                    var cEventGraphics = new CalendarEventGraphicsWraper(this, cEvent, dayOfWeekk);

                    cEventGraphics.DrawCalanderEvent();
                }
            }
        }


        public int GetHeightBasedOfCalendarEvent(CalendarEvent calendarEvent) {
            const int minutesInOneDay = 60 * 24;

            var duration = (calendarEvent.End - calendarEvent.Start).TotalMinutes;

            return GetResponsiveValue(duration, minutesInOneDay, Height);
        }

        public int GetYPosFromCalendarEvent(CalendarEvent calendarEvent) {
            const double minInOneDay = 60 * 24;

            double minutes = calendarEvent.Start.Hour * 60;
            minutes += calendarEvent.Start.Minute;

            return GetResponsiveValue(minutes, minInOneDay, Height);
        }

        //Converts the value in px to a value in %
        private static int GetResponsiveValue(double valuePart, double valueWhole, double value) {
            var yPosInPercent = valuePart / valueWhole;
            return Convert.ToInt32(yPosInPercent * value);
        }

        public void SaveImage() {
            Console.WriteLine(Image.Height);
            Console.WriteLine(Image.Width);
            Image.Save(OutPutDir, ImageFormat.Png);
            Image.Dispose();
        }

    }

}