﻿using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public class BookingCompleted : IEvent
    {
        public int ApartmentsId { get; set; }
    }
}
