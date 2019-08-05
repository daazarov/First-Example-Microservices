using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
    public class PayInvoice : ICommand
    {
        public Guid ReservationId { get; set; }
        public int ApartmentsId { get; set; }
        public int InvoiceId { get; set; }
    }
}
