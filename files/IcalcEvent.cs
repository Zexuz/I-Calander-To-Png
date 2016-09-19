using System;
using System.Collections.Generic;

namespace ICalendarToPng.files {

    public class IcalcEvent : Event {

        public string Uid { get; private set; }

        public Repeater Repeater { get; set; }

        public IcalcEvent(string uid) {
            Uid = uid;
        }

        public Event toEvent() {
            return new Event {
                Start = Start,
                End = End,
                Location = Location,
                Subject = Subject
            };
        }

        public List<CalendarEvent> GetRepeatedEvent() {
            throw new NotImplementedException();
        }

    }

}