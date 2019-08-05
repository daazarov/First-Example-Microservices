using NServiceBus;
using System;

namespace Messages.Commands
{
    public class PayInvoice : ICommand
    {
        public Guid ReservationId { get; set; }
        public int ApartmentsId { get; set; }
        public int InvoiceId { get; set; }
    }
}
