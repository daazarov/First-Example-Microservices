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
    public class ApartmentsReservedHandler : IHandleMessages<ApartmentsReserved>
    {
        private readonly ILog _logger;
        public ApartmentsReservedHandler()
        {
            _logger = LogManager.GetLogger<ApartmentsReservedHandler>();
        }

        public Task Handle(ApartmentsReserved message, IMessageHandlerContext context)
        {
            _logger.Info($"ApartmentsReserved message recived, apartments id: {message.ApartmentsId}, reservation id: {message.ReservationId}");

            Program.BookingList.Find(_ => _.ApartmentsId == message.ApartmentsId).ReservationId = message.ReservationId;
            return Task.CompletedTask;
        }
    }
}
