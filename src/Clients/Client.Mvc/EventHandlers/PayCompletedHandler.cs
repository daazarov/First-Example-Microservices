using Client.Mvc.Data;
using Client.Mvc.Models.Enums;
using Messages.Events;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Client.Mvc.EventHandlers
{
    public class PayCompletedHandler : IHandleMessages<PayCompleted>
    {
        private readonly ILog _logger;

        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly BookingFakeRepository _repository;

        public PayCompletedHandler(IHubContext<NotificationHub> hubContext, BookingFakeRepository repository)
        {
            _logger = LogManager.GetLogger<PayCompletedHandler>();

            this._hubContext = hubContext;
            this._repository = repository;
        }

        public Task Handle(PayCompleted message, IMessageHandlerContext context)
        {
            _logger.Info($"PayCompleted message recived, reservation id: {message.ReservationId}, invoice id: {message.InvoiceId}");
            _hubContext.Clients.All.SendAsync(NotificationTypes.primary.ToString(), $"PayCompleted message recived, reservation id: {message.ReservationId}, invoice id: {message.InvoiceId}");

            _repository.Find(_ => _.ReservationId == message.ReservationId && _.Payment.InvoiceId == message.InvoiceId).Payment.IsPaid = true;
            return Task.CompletedTask;
        }
    }
}
