using NServiceBus;
using System;

namespace Messages.Events
{
    public class PayCompleted : IEvent
    {
        public Guid ReservationId { get; set; }
        public int InvoiceId { get; set; }
        public int ApartmentsId { get; set; }
    }
}
