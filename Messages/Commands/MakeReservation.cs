using NServiceBus;
using System;

namespace Messages.Commands
{
    public class MakeReservation : ICommand
    {
        public int ApartmentsId { get; set; }

    }
}
