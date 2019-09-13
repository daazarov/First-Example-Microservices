using Client.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Mvc.Data
{
    public class BookedApartamentsFakeRepository
    {
        private List<int> _bookedList;

        private List<int> BookedList
        {
            get
            {
                if (_bookedList == null)
                    _bookedList = new List<int>();
                return _bookedList;
            }
        }

        public int Count
        {
            get => BookedList.Count;
        }

        public void Add(int booking)
        {
            BookedList.Add(booking);
        }

        public bool Contains(int booking)
        {
            return BookedList.Contains(booking);
        }

        public void Clean()
        {
            BookedList.Clear();
        }
    }
}
