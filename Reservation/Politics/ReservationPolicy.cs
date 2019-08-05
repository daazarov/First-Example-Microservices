using Messages.Commands;
using Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Politics
{
    public class ReservationPolicy : Saga<ReservationPolicyData>, 
        IHandleMessages<PayCompleted>, 
        IAmStartedByMessages<MakeReservation>
    {
        private readonly ILog _logger;
        public ReservationPolicy()
        {
            _logger = LogManager.GetLogger<ReservationPolicy>();
        }

        public Task Handle(PayCompleted message, IMessageHandlerContext context)
        {
            _logger.Info($"PayCompleted message recived, reservation id: {message.ReservationId}");
            Data.IsPaid = true;

            return ProcessOrder(context);
        }

        public Task Handle(MakeReservation message, IMessageHandlerContext context)
        {
            _logger.Info($"MakeReservation message recived, apartments id: {message.ApartmentsId}");
            var @event = new ApartmentsReserved();

            Guid ReservationId = Guid.NewGuid();

            @event.ApartmentsId = message.ApartmentsId;
            @event.ReservationId = ReservationId;
            Data.IsReserved = true;

            _logger.Info($"ApartmentsReserved event publishing, apartments id: {message.ApartmentsId}, reservation id: {ReservationId}");
            return context.Publish(@event);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ReservationPolicyData> mapper)
        {
            mapper.ConfigureMapping<PayCompleted>(message => message.ApartmentsId)
                .ToSaga(sagaData => sagaData.ApartmentsId);
            mapper.ConfigureMapping<MakeReservation>(message => message.ApartmentsId)
                .ToSaga(sagaData => sagaData.ApartmentsId);
        }

        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsPaid && Data.IsReserved)
            {
                var @event = new BookingCompleted();
                @event.ApartmentsId = Data.ApartmentsId;

                _logger.Info($"BookingCompleted event publishing, apartments id: {Data.ApartmentsId}");
                await context.Publish(@event);
                MarkAsComplete();
            }
        }
    }
}
