using Client.Mvc.Data;
using Client.Mvc.Models.Enums;
using Messages.Events;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Client.Mvc.EventHandlers
{
    public class BookingCompletedHandler : IHandleMessages<BookingCompleted>
    {
        private readonly ILog _logger;

        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly BookingFakeRepository _repository;

        public BookingCompletedHandler(IHubContext<NotificationHub> hubContext, BookingFakeRepository repository)
        {
            _logger = LogManager.GetLogger<BookingCompletedHandler>();

            this._hubContext = hubContext;
            this._repository = repository;
        }

        public Task Handle(BookingCompleted message, IMessageHandlerContext context)
        {
            _logger.Info($"BookingCompleted message recived, apartments id: {message.ApartmentsId}");
            _hubContext.Clients.All.SendAsync(NotificationTypes.success.ToString(), $"BookingCompleted message recived, apartments id: {message.ApartmentsId}");

            _repository.Find(_ => _.ApartmentsId == message.ApartmentsId).IsCompleted = true;
            return Task.CompletedTask;
        }
    }
}
