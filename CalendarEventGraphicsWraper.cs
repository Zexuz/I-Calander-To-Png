using System.Drawing;

namespace ICalendarToPng {

    public class CalendarEventGraphicsWraper {

        private readonly Image _image;
        private readonly CalendarEvent _calendarEvent;

        private readonly int _dayOfWeek;

        public Bitmap BitmapImage;

        public CalendarEventGraphicsWraper(Image image, CalendarEvent calendarEvent, int dayOfWeek) {
            //IDE DON*T LET ME HAVE A ROW HERE...

            _image = image;
            _calendarEvent = calendarEvent;
            _dayOfWeek = dayOfWeek;
        }


        public void DrawCalanderEvent() {
            var rect = new Rectangle(
                GetX(),
                GetY(),
                GetWidth(),
                GetHeight()
            );

            using (var g = Graphics.FromImage(_image.BitmapImage)) {
                g.DrawRectangle(new Pen(Color.Black), rect);
                DrawStartTime();
                DrawEndTime();
            }
        }

        private void DrawEndTime() {
            var x = (int) (GetX() + GetWidth() - (0.0375 * _image.HeightMinusMargin));
            var y = GetY() + GetHeight() - 13;

            DrawString(_calendarEvent.End.ToShortTimeString(), x, y);
        }

        private void DrawStartTime() {
            var x = GetX();
            var y = GetY();

            DrawString(_calendarEvent.Start.ToShortTimeString(), x, y);
        }

        private void DrawString(string str, int x, int y, int font = 8) {
            using (var g = Graphics.FromImage(_image.BitmapImage)) {
                //Empty comment...

                g.DrawString(str, new Font("Arial", font), new SolidBrush(Color.Black), x, y);
            }
        }


        private int GetX() {
            return _image.Width / 7 * _dayOfWeek;
        }

        private int GetY() {
            return _image.GetYPosFromCalendarEvent(_calendarEvent) + _image.MarginTop;
        }

        private int GetHeight() {
            return _image.GetHeightBasedOfCalendarEvent(_calendarEvent);
        }

        private int GetWidth() {
            return _image.Width / 7 - 10;
        }

    }

}