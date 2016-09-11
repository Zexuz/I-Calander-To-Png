using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ICalendarToPng {

    public class CalendarEventGraphicsWraper {

        private readonly int _height;
        private readonly int _width;
        private int _scale;
        private string _outPutDir = @"../../res/image.png";

        private Bitmap _image;

        public CalendarEventGraphicsWraper(int height, int width, int scale) {
            _height = height;
            _width = width;
            _scale = scale;

            _image = new Bitmap(_width, _height);

            using (var g = Graphics.FromImage(_image)) {
                g.Clear(Color.White);
            }
        }

        public void DrawCalanderEvent(CalendarEvent calendarEvent) {
            var rect = new Rectangle(10, GetYPosFromCalendarEvent(calendarEvent), (_width / 7) - 10,
                GetHeightBasedOfCalendarEvent(calendarEvent));

            using (var g = Graphics.FromImage(_image)) {
                g.DrawRectangle(new Pen(Color.Black), rect);
            }
        }

        public int GetHeightBasedOfCalendarEvent(CalendarEvent calendarEvent) {
            const int minutesInOneDay = 60 * 24;

            var duration = calendarEvent.End - calendarEvent.Start;
            var heightInPercent = duration.TotalMinutes / minutesInOneDay;

            return Convert.ToInt32(_height * heightInPercent);
        }


        public int GetYPosFromCalendarEvent(CalendarEvent calendarEvent) {
            const double minInOneDay = 60 * 24;

            double minutes = calendarEvent.Start.Hour * 60;
            minutes += calendarEvent.Start.Minute;

            var yPosInPercent = minutes / minInOneDay;

            return Convert.ToInt32(yPosInPercent * _height);
        }


        public void SaveImage() {
            _image.Save(_outPutDir, ImageFormat.Png);
            _image.Dispose();
        }

    }

}