using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ICalendarToPng {

    public class Image {

        private const string OutPutDir = @"../../res/";

        public Bitmap BitmapImage { get; private set; }
        public int HeightMinusMargin { get; private set; }
        public int Width { get; private set; }
        public int MarginTop { get; private set; }
        public int Height { get; private set; }


        public Image(int height, int width, int marginTop = 0) {
            Width = width;
            Height = height;
            MarginTop = marginTop;
            HeightMinusMargin = height - marginTop;

            BitmapImage = new Bitmap(Width, Height);

            ClearAndPaintHourLines();
            PaintDayLines();
        }

        public void PaintDayLines() {}

        public void ClearAndPaintHourLines() {
            using (var g = Graphics.FromImage(BitmapImage)) {
                g.Clear(Color.White);

                var semiTransPen = new Pen(Color.FromArgb(64, 0, 0, 255), 1);
                PaintStartLine();

                for (var i = 1; i < 24; i++) {
                    var yCord = GetResponsiveValue(i, 24, HeightMinusMargin) + MarginTop;
                    g.DrawLine(semiTransPen, 0, yCord, Width, yCord);
                }
            }
        }

        public void PaintDays(List<Day> days) {
            foreach (var day in days) {
                if (day == null || day.CalendarEvents == null) {
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

        public void WriteWeek(Week week) {
            using (var g = Graphics.FromImage(BitmapImage)) {
                g.DrawString($"V. {week.WeekNr}", new Font("Arial", 16), new SolidBrush(Color.Black), Width / 2, 10);

                PaintDays(week.Days);
                SaveImage(week.WeekNr);
            }
        }


        public int GetHeightBasedOfCalendarEvent(CalendarEvent calendarEvent) {
            const int minutesInOneDay = 60 * 24;

            var duration = (calendarEvent.End - calendarEvent.Start).TotalMinutes;

            return GetResponsiveValue(duration, minutesInOneDay, HeightMinusMargin);
        }

        public int GetYPosFromCalendarEvent(CalendarEvent calendarEvent) {
            const double minInOneDay = 60 * 24;

            double minutes = calendarEvent.Start.Hour * 60;
            minutes += calendarEvent.Start.Minute;

            return GetResponsiveValue(minutes, minInOneDay, HeightMinusMargin);
        }

        private void PaintStartLine() {
            using (var g = Graphics.FromImage(BitmapImage)) {
                var solidPen = new Pen(Color.Black, 3);

                var yCord = GetResponsiveValue(0, 24, HeightMinusMargin) + MarginTop;
                g.DrawLine(solidPen, 0, yCord, Width, yCord);
            }
        }

        //Converts the value in px to a value in %
        private static int GetResponsiveValue(double valuePart, double valueWhole, double value) {
            var yPosInPercent = valuePart / valueWhole;
            return Convert.ToInt32(yPosInPercent * value);
        }

        public void SaveImage(int weekNr) {
            Console.WriteLine(BitmapImage.Height);
            Console.WriteLine(BitmapImage.Width);
            BitmapImage.Save($"{OutPutDir}{weekNr}.png", ImageFormat.Png);
            BitmapImage.Dispose();
        }

    }

}