using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class BookingModel
    {
        public int ApartmentsId { get; set; }
        public Guid ReservationId { get; set; }
        public PaymentModel Payment { get; set; }
        public bool IsCompleted { get; set; }
    }
}
