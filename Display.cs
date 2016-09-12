using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

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

            using (var g = Graphics.FromImage(Image)) {
                g.Clear(Color.White);
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