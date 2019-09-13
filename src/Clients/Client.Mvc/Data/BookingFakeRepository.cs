using Client.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Mvc.Data
{
    public class BookingFakeRepository
    {
        private List<BookingModel> _bookingList;

        private List<BookingModel> BookingList
        {
            get
            {
                if (_bookingList == null)
                    _bookingList = new List<BookingModel>();
                return _bookingList;
            }
        }

        public void Add(BookingModel model)
        {
            BookingList.Add(model);
        }

        public BookingModel Find(Func<BookingModel, bool> predicate)
        {
            return BookingList.Where(predicate).FirstOrDefault();
        }
    }
}
