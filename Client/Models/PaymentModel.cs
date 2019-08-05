using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class PaymentModel
    {
        public int InvoiceId { get; set; }
        public bool IsPaid { get; set; }
    }
}
