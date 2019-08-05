using NServiceBus;
using System;

namespace Messages.Events
{
    public class InvoiceCreated : IEvent
    {
        public int InvoiceId { get; set; }
        public Guid ReservationId { get; set; }
    }
}
