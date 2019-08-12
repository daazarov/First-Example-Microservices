using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Billing.CommandHandlers
{
    public class PayInvoiceHandler : IHandleMessages<PayInvoice>
    {
        private readonly ILog _logger;
        public PayInvoiceHandler()
        {
            _logger = LogManager.GetLogger<PayInvoiceHandler>();
        }

        public Task Handle(PayInvoice message, IMessageHandlerContext context)
        {
            _logger.Info($"(PayInvoice command recived, invoive id: {message.InvoiceId}");

            var @event = new PayCompleted();
            @event.ReservationId = message.ReservationId;
            @event.ApartmentsId = message.ApartmentsId;
            @event.InvoiceId = message.InvoiceId;

            _logger.Info($"(PayCompleted event sending, invoive id: {message.InvoiceId}, reservation id: {message.ReservationId}");
            return context.Publish(@event);
        }
    }
}
