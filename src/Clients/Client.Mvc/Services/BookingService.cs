using Client.Mvc.Data;
using Client.Mvc.Models;
using Client.Mvc.Models.Enums;
using Messages.Commands;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Mvc.Services
{
    public class BookingService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ReusedComponent _component;
        private readonly BookingFakeRepository _repository;
        private readonly BookedApartamentsFakeRepository _bookedRepository;

        public BookingService(IHubContext<NotificationHub> hubContext, ReusedComponent component, BookingFakeRepository repository, BookedApartamentsFakeRepository bookedRepository)
        {
            this._hubContext = hubContext;
            this._component = component;
            this._repository = repository;
            this._bookedRepository = bookedRepository;
        }

        public async Task NewBooking()
        {
            CleanData();

            int Apartments_id = new Random().Next(1, 999);

            _repository.Add(new BookingModel { ApartmentsId = Apartments_id });
            if (_bookedRepository.Contains(Apartments_id))
            {
                await _hubContext.Clients.All.SendAsync(NotificationTypes.danger.ToString(), $"Apartments {Apartments_id} is already booked");
                return;
            }
            _bookedRepository.Add(Apartments_id);

            var command = new BookAppartment();
            command.ApartmentsId = Apartments_id;

            await _hubContext.Clients.All.SendAsync(NotificationTypes.primary.ToString(), $"Sending BookApartments command, ApartmentsId = {command.ApartmentsId}");
            await _component.SendCommand(command);
        }

        public async Task PayInvoice()
        {
            var reservation = _repository.Find(_ => !_.IsCompleted && _.Payment != null && !_.Payment.IsPaid);

            if (reservation == null)
            {
                await _hubContext.Clients.All.SendAsync(NotificationTypes.warning.ToString(), "Not found reserved apartments");
                return;
            }

            var command = new PayInvoice();
            command.ReservationId = reservation.ReservationId;
            command.InvoiceId = reservation.Payment.InvoiceId;
            command.ApartmentsId = reservation.ApartmentsId;

            await _hubContext.Clients.All.SendAsync(NotificationTypes.primary.ToString(), $"Sending PayInvoice command, invoice id = {command.InvoiceId}, reservation id = { command.ReservationId}");
            await _component.SendCommand(command);
        }

        private void CleanData()
        {
            if (_bookedRepository.Count > 50)
                _bookedRepository.Clean();
        }
    }
}
