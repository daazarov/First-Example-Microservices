using Client.Mvc.Data;
using Client.Mvc.Models.Enums;
using Messages.Events;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Client.Mvc.EventHandlers
{
    public class ApartmentsReservedHandler : IHandleMessages<ApartmentsReserved>
    {
        private readonly ILog _logger;

        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly BookingFakeRepository _repository;

        public ApartmentsReservedHandler(IHubContext<NotificationHub> hubContext, BookingFakeRepository repository)
        {
            this._hubContext = hubContext;
            this._repository = repository;

            _logger = LogManager.GetLogger<ApartmentsReservedHandler>();
        }

        public Task Handle(ApartmentsReserved message, IMessageHandlerContext context)
        {
            _logger.Info($"ApartmentsReserved message recived, apartments id: {message.ApartmentsId}, reservation id: {message.ReservationId}");
            _hubContext.Clients.All.SendAsync(NotificationTypes.primary.ToString(), $"ApartmentsReserved message recived, apartments id: {message.ApartmentsId}, reservation id: {message.ReservationId}");

            _repository.Find(_ => _.ApartmentsId == message.ApartmentsId).ReservationId = message.ReservationId;
            return Task.CompletedTask;
        }
    }
}
