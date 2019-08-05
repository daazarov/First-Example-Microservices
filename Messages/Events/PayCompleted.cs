using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public class PayCompleted : IEvent
    {
        public Guid ReservationId { get; set; }
        public int InvoiceId { get; set; }
        public int ApartmentsId { get; set; }
    }
}
