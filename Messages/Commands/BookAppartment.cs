using NServiceBus;

namespace Messages.Commands
{
    public class BookAppartment : ICommand
    {
        public int ApartmentsId { get; set; }
    }
}
