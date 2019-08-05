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
    public class PayCompletedHandler : IHandleMessages<PayCompleted>
    {
        private readonly ILog _logger;
        public PayCompletedHandler()
        {
            _logger = LogManager.GetLogger<PayCompletedHandler>();
        }

        public Task Handle(PayCompleted message, IMessageHandlerContext context)
        {
            _logger.Info($"PayCompleted message recived, reservation id: {message.ReservationId}, invoice id: {message.InvoiceId}");

            Program.BookingList.Find(_ => _.ReservationId == message.ReservationId && _.Payment.InvoiceId == message.InvoiceId).Payment.IsPaid = true;
            return Task.CompletedTask;
        }
    }
}
