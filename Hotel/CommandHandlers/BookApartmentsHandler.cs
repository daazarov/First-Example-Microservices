using Messages.Commands;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.CommandHandlers
{
    public class BookApartmentsHandler : IHandleMessages<BookAppartment>
    {
        private readonly ILog _logger;
        public BookApartmentsHandler(/*ILogger<BookApartmentsHandler> logger*/)
        {
            _logger = LogManager.GetLogger<BookApartmentsHandler>();
        }
        public Task Handle(BookAppartment message, IMessageHandlerContext context)
        {
            _logger.Info($"Recived BookAppartment, apartments id: {message.ApartmentsId}");

            var command = new MakeReservation();
            command.ApartmentsId = message.ApartmentsId;

            _logger.Info($"Sending MakeReservation, apartments id: {message.ApartmentsId}");

            return context.Send(command);
        }
    }
}
