using Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Billing.EventHandlers
{
    public class ApartmentsReservedHandler : IHandleMessages<ApartmentsReserved>
    {
        private readonly ILog _logger;
        private static int invoiceCounter = 1;
        public ApartmentsReservedHandler()
        {
            _logger = LogManager.GetLogger<ApartmentsReservedHandler>();
        }

        public Task Handle(ApartmentsReserved message, IMessageHandlerContext context)
        {
            _logger.Info($"ApartmentsReserved message recived, apartments id: {message.ApartmentsId}, reservation id: {message.ReservationId}");
            var @event = new InvoiceCreated();
            @event.InvoiceId = invoiceCounter;
            @event.ReservationId = message.ReservationId;

            _logger.Info($"InvoiceCreated event publishing, reservation id: {message.ReservationId}, apartments id: {message.ApartmentsId}, invoice id: {@event.InvoiceId}");
            invoiceCounter++;
            return context.Publish(@event);
        }
    }
}
