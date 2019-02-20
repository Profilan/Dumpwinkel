using Dumpwinkel.Logic.Events;
using Profilan.SharedKernel;
using System;
using System.Collections.Generic;

namespace Dumpwinkel.Logic.Models
{
    public class Event : Entity<Guid>
    {
        public virtual Dumpstore Dumpstore { get; protected set; }
        public virtual DateTimeRange TimeRange { get; protected set; }
        public virtual int MaximumNumberOfVisitors { get; protected set; }

        public virtual Theme Theme { get; set; }

        protected Event()
        {

        }

        public Event(Guid id) : base(id)
        {

        }

        public static Event Create(Dumpstore dumpstore,
            DateTime startTime,
            DateTime endTime,
            int maximumNumberOfVisitors,
            Theme theme = null
            )
        {
            var newEvent = new Event(Guid.NewGuid());
            newEvent.TimeRange = new DateTimeRange(startTime, endTime);
            newEvent.MaximumNumberOfVisitors = maximumNumberOfVisitors;
            newEvent.Dumpstore = dumpstore;
            newEvent.Theme = theme;

            return newEvent;
        }

        public static IList<Event> CreateRange(Dumpstore dumpstore, DateTime startDate, DateTime endDate, Interval interval, int maxPersons, Theme theme = null)
        {
            var duration = (endDate - startDate).Duration();

            var numberOfEvents = Convert.ToInt32(Math.Floor(duration.TotalMinutes / interval.Minutes));

            var events = new List<Event>();
            for (int i = 0; i < numberOfEvents; i++)
            {
                events.Add(Event.Create(dumpstore, startDate.AddMinutes(i * interval.Minutes), startDate.AddMinutes((i + 1) * interval.Minutes), maxPersons, theme));
            }

            return events;
        }

        public virtual void UpdateMaximumNumberOfVisitors(int newMaximum)
        {
            if (newMaximum == MaximumNumberOfVisitors) return;

            MaximumNumberOfVisitors = newMaximum;

            var eventUpdatedEvent = new EventUpdatedEvent(this);
            DomainEvents.Raise(eventUpdatedEvent);
        }

    }
}
