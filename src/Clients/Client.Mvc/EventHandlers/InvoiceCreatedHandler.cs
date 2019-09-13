using Client.Mvc.Data;
using Client.Mvc.Models.Enums;
using Messages.Events;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Client.Mvc.EventHandlers
{
    public class InvoiceCreatedHandler : IHandleMessages<InvoiceCreated>
    {
        private readonly ILog _logger;

        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly BookingFakeRepository _repository;

        public InvoiceCreatedHandler(IHubContext<NotificationHub> hubContext, BookingFakeRepository repository)
        {
            _logger = LogManager.GetLogger<InvoiceCreatedHandler>();

            this._hubContext = hubContext;
            this._repository = repository;
        }

        public Task Handle(InvoiceCreated message, IMessageHandlerContext context)
        {
            _logger.Info($"InvoiceCreated message recived, invoice id: {message.InvoiceId}");
            _hubContext.Clients.All.SendAsync(NotificationTypes.primary.ToString(), $"InvoiceCreated message recived, invoice id: {message.InvoiceId}");


            var booking = _repository.Find(_ => _.ReservationId == message.ReservationId);
            if (booking == null)
            {
                _logger.Error($"Booking with reservation id '{message.ReservationId}' was not found");
                _hubContext.Clients.All.SendAsync(NotificationTypes.danger.ToString(), $"Booking with reservation id '{message.ReservationId}' was not found");
                return Task.CompletedTask;
            }

            booking.Payment = new Models.PaymentModel { InvoiceId = message.InvoiceId };
            return Task.CompletedTask;
        }
    }
}
