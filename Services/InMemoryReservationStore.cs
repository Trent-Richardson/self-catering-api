using System.Collections.Generic;
using System.Linq;
using SelfCatering;
using System;

namespace SelfCatering 
{
    public class InMemoryReservationStore : IReservationStore
    {
        private List<Reservation> _reservations = new List<Reservation>();

        public InMemoryReservationStore()
        {
            // starter data
            _reservations = new List<Reservation>
            {
                new Reservation 
                {
                    Id = 1,
                    Address = new AddressDetails 
                    {
                        Id = 1,
                        Address = "12 Barbados St"
                    },
                    InTime = new DateTime(1990,1,1),
                    OutTime = new DateTime(1991,1,1)
                },
                new Reservation 
                {
                    Id = 2,
                    Address = new AddressDetails 
                    {
                        Id = 1,
                        Address = "12 Barbados St"
                    },
                    InTime = new DateTime(1991,1,1),
                    OutTime = new DateTime(1992,1,1)
                },
            };
        }

        public List<Reservation> GetReservations() => _reservations;
        
        public int? BookReservation(Reservation r) 
        {
            // todo: enhance validation here ...
            if(_reservations.Any(x => x.Address.Id == r.Address.Id && (r.InTime >= x.InTime && r.InTime <= x.OutTime) 
                                        && (r.OutTime >= x.InTime && r.OutTime <= x.OutTime)))
                return null;

            r.Id = _reservations.Max(x => x.Id) + 1;
            System.Console.WriteLine(r.Id);
            _reservations.Add(r);
            return (int) r.Id;
        }

        public void CancelReservation(int reservationId) => _reservations.Remove(_reservations.First(x => x.Id == reservationId));

        public void UpdateReservation(DateTime inTime, DateTime outTime) 
        { 
            // todo
        }
        public void AddReview(string review) 
        {
            // todo
        }
    }
}
