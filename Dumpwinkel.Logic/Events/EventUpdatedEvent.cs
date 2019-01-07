using Dumpwinkel.Logic.Models;
using Profilan.SharedKernel;
using System;

namespace Dumpwinkel.Logic.Events
{
    public class EventUpdatedEvent : IDomainEvent
    {
        public EventUpdatedEvent(Event eventItem)
            : this()
        {
            EventUpdated = eventItem;
        }
        public EventUpdatedEvent()
        {
            this.Id = Guid.NewGuid();
            DateTimeEventOccurred = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public DateTime DateTimeEventOccurred { get; private set; }
        public Event EventUpdated { get; private set; }
    }
}
