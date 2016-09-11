using System;

namespace ICalendarToPng {

    public class CalendarEvent {

        public DateTime Start;
        public DateTime End;

        public string Location;
        public string Subject;


        public override string ToString() {
            return "Start: " + Start + ", " + "End: " + End + ", " + "Location: " + Location + ", " + "Subject: " +
                   Subject;
        }

    }

}