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
    public class BookingCompletedHandler : IHandleMessages<BookingCompleted>
    {
        private readonly ILog _logger;
        public BookingCompletedHandler()
        {
            _logger = LogManager.GetLogger<BookingCompletedHandler>();
        }

        public Task Handle(BookingCompleted message, IMessageHandlerContext context)
        {
            _logger.Info($"BookingCompleted message recived, apartments id: {message.ApartmentsId}");

            Program.BookingList.Find(_ => _.ApartmentsId == message.ApartmentsId).IsCompleted = true;
            return Task.CompletedTask;
        }
    }
}
