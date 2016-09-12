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

        public void DrawDay(Day day) {}


        public void DrawCalanderEvent(CalendarEvent calendarEvent, int dayOfWeek) {
            Console.WriteLine(calendarEvent.ToString());

            var rect = new Rectangle(
                _display.Width / 7 * dayOfWeek,
                _display.GetYPosFromCalendarEvent(calendarEvent),
                _display.Width / 7 - 10,
                _display.GetHeightBasedOfCalendarEvent(calendarEvent)
            );

            var font = new Font("Arial", 8);
            var brush = new SolidBrush(Color.Black);


            using (var g = Graphics.FromImage(_display.Image)) {
                g.DrawRectangle(new Pen(Color.Black), rect);
                g.DrawString(
                    calendarEvent.Start.ToShortTimeString(),
                    font, brush,
                    _display.Width / 7 * dayOfWeek,
                    _display.GetYPosFromCalendarEvent(calendarEvent)
                );
                g.DrawString(
                    calendarEvent.End.ToShortTimeString(),
                    font, brush,
                    _display.Width / 7 * dayOfWeek,
                    _display.GetYPosFromCalendarEvent(calendarEvent)
                    +_display.GetHeightBasedOfCalendarEvent(calendarEvent) -15
                );
            }
        }

    }

}























