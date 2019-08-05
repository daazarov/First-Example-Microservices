using Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.Extensions.Configuration;

namespace Client
{
    class Program
    {
        private static ILog log = LogManager.GetLogger<Program>();
        public static List<BookingModel> BookingList;
        private static List<int> bookedApartments;
        private static IEndpointInstance _instance;
        private static IConfiguration _configuration;

        static async Task Main(string[] args)
        {
            Console.Title = "Client";

            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var endPointConfiguration = Helper.ConfigureEndpoint(_configuration);
            _instance = await Endpoint.Start(endPointConfiguration).ConfigureAwait(false);

            await Menu();
        }

        static async Task Menu()
        {
            while (true)
            {
                log.Info("\nPress: \n\t'B' - for the booking Apartments\n\t'P' - for pay\n\t'Q' - exit");
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.B:
                        await Booking(_instance);
                        break;
                    case ConsoleKey.P:
                        await Pay(_instance);
                        break;
                    case ConsoleKey.Q:
                        return;
                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }

        static async Task Booking(IEndpointInstance endpointInstance)
        {
            if (BookingList == null)
               BookingList = new List<BookingModel>();
            if (bookedApartments == null)
                bookedApartments = new List<int>();

            int Apartments_id = new Random().Next(1, 999);

            BookingList.Add(new BookingModel { ApartmentsId = Apartments_id });
            if (bookedApartments.Contains(Apartments_id))
            {
                log.Error($"Apartments {Apartments_id} is already booked");
                return;
            }
            bookedApartments.Add(Apartments_id);

            var command = new BookAppartment();
            command.ApartmentsId = Apartments_id;

            log.Info($"Sending BookApartments command, ApartmentsId = {command.ApartmentsId}");
            await endpointInstance.Send(command).ConfigureAwait(false);
        }

        static async Task Pay(IEndpointInstance endpointInstance)
        {
            if (BookingList == null)
            {
                log.Error($"Not found reserved apartments");
            }

            var reservation = BookingList.Find(_ => !_.IsCompleted && _.Payment != null && !_.Payment.IsPaid);

            var command = new PayInvoice();
            command.ReservationId = reservation.ReservationId;
            command.InvoiceId = reservation.Payment.InvoiceId;
            command.ApartmentsId = reservation.ApartmentsId;

            log.Info($"Sending PayInvoice command, invoice id = {command.InvoiceId}, reservation id = { command.ReservationId}");
            await endpointInstance.Send(command);
        }
    }
}
