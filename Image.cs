using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using ICalendarToPng.files;

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
                if (day == null || day.Events == null) {
                    //TODO draw "nothing here" on img
                    continue;
                }


                foreach (var ev in day.Events) {
                    var dayOfWeekk = (int) day.Events[0].Start.DayOfWeek;
                    var eventGraphics = new EventGraphicsWraper(this, ev, dayOfWeekk);

                    eventGraphics.DrawEvent();
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


        public int GetHeightBasedOfEvent(Event ev) {
            const int minutesInOneDay = 60 * 24;

            var duration = (ev.End - ev.Start).TotalMinutes;

            return GetResponsiveValue(duration, minutesInOneDay, HeightMinusMargin);
        }

        public int GetYPosFromEvent(Event ev) {
            const double minInOneDay = 60 * 24;

            double minutes = ev.Start.Hour * 60;
            minutes += ev.Start.Minute;

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
            BitmapImage.Save($"{OutPutDir}{weekNr}.png", ImageFormat.Png);
            BitmapImage.Dispose();
        }

    }

}