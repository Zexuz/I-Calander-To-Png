using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ICalendarToPng {

    public class CalendarEventGraphicsWraper {

        private Display _display;

        public Bitmap Image;
        public CalendarEventGraphicsWraper(Display display) {
            //IDE DON*T LET ME HAVE A ROW HERE...


            _display = display;
            using (var g = Graphics.FromImage(_display.Image)) {
                g.Clear(Color.White);
            }
        }

        public void DrawCalanderEvent(CalendarEvent calendarEvent) {
            Console.WriteLine(calendarEvent.ToString());

            var rect = new Rectangle(
                10,
                _display.GetYPosFromCalendarEvent(calendarEvent),
                _display.Width / 7 - 10,
                _display.GetHeightBasedOfCalendarEvent(calendarEvent)
            );


            using (var g = Graphics.FromImage(_display.Image)) {
                g.DrawRectangle(new Pen(Color.Black), rect);
            }
        }

    }

}