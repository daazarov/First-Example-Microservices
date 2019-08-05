using NServiceBus;

namespace Messages.Commands
{
    public class MakeReservation : ICommand
    {
        public int ApartmentsId { get; set; }

    }
}
