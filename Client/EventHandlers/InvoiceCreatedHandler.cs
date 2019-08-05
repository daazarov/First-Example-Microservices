using Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventHandlers
{
    public class InvoiceCreatedHandler : IHandleMessages<InvoiceCreated>
    {
        private readonly ILog _logger;
        public InvoiceCreatedHandler()
        {
            _logger = LogManager.GetLogger<InvoiceCreatedHandler>();
        }

        public Task Handle(InvoiceCreated message, IMessageHandlerContext context)
        {
            _logger.Info($"InvoiceCreated message recived, invoice id: {message.InvoiceId}");

            var booking = Program.BookingList.Find(_ => _.ReservationId == message.ReservationId);
            if (booking == null)
            {
                _logger.Error($"Booking with reservation id '{message.ReservationId}' was not found");
                return Task.CompletedTask;
            }

            booking.Payment = new Models.PaymentModel { InvoiceId = message.InvoiceId };
            return Task.CompletedTask;
        }
    }
}
