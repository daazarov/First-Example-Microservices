using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public class ApartmentsReserved : IEvent
    {
        public int ApartmentsId { get; set; }
        public Guid ReservationId { get; set; }
    }
}
