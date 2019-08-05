using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
    public class BookAppartment : ICommand
    {
        public int ApartmentsId { get; set; }
    }
}
