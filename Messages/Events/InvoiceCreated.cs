using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public class InvoiceCreated : IEvent
    {
        public int InvoiceId { get; set; }
        public Guid ReservationId { get; set; }
    }
}
