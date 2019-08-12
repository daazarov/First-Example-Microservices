using NServiceBus;
using System;

namespace Messages.Events
{
    public class ApartmentsReserved : IEvent
    {
        public int ApartmentsId { get; set; }
        public Guid ReservationId { get; set; }
    }
}
