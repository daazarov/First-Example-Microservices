using NServiceBus;

namespace Messages.Events
{
    public class BookingCompleted : IEvent
    {
        public int ApartmentsId { get; set; }
    }
}
