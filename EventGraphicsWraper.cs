using System.Drawing;

using ICalendarToPng.files;

namespace ICalendarToPng {

    public class EventGraphicsWraper {

        private readonly Image _image;
        private readonly Event _event;

        private readonly int _dayOfWeek;

        public Bitmap BitmapImage;

        public EventGraphicsWraper(Image image, Event ev, int dayOfWeek) {
            //IDE DON*T LET ME HAVE A ROW HERE...

            _image = image;
            _event = ev;
            _dayOfWeek = dayOfWeek;
        }


        public void DrawEvent() {
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

                //delete this code
                var x = (int) (GetX() + GetWidth() / 2 + 10 - (0.0375 * _image.HeightMinusMargin));
                var y = GetY() + GetHeight() - 13;
                DrawString(_event.Start.DayOfWeek.ToString(), x, y);
                //-------
            }
        }

        private void DrawEndTime() {
            var x = (int) (GetX() + GetWidth() - (0.0375 * _image.HeightMinusMargin));
            var y = GetY() + GetHeight() - 13;

            DrawString(_event.End.ToShortTimeString(), x, y);
        }

        private void DrawStartTime() {
            var x = GetX();
            var y = GetY();

            DrawString(_event.Start.ToShortTimeString(), x, y);
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
            return _image.GetYPosFromEvent(_event) + _image.MarginTop;
        }

        private int GetHeight() {
            return _image.GetHeightBasedOfEvent(_event);
        }

        private int GetWidth() {
            return _image.Width / 7 - 10;
        }

    }

}