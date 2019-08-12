using NServiceBus;

namespace Reservation.Politics
{
    public class ReservationPolicyData : ContainSagaData
    {
        public int ApartmentsId { get; set; }
        public bool IsPaid { get; set; }
        public bool IsReserved { get; set; }
    }
}
