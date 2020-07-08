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
                        Address = "13 Barbados St"
                    },
                    InTime = new DateTime(1991,1,1),
                    OutTime = new DateTime(1992,1,1)
                }
            };
        }

        private bool CheckConflictingBooking(Reservation r) => _reservations.Any(x => x.Address.Id == r.Address.Id && (r.InTime >= x.InTime && r.InTime <= x.OutTime) 
                                        && (r.OutTime >= x.InTime && r.OutTime <= x.OutTime));

        public List<Reservation> GetReservations() => _reservations;
        
        public int? BookReservation(Reservation r) 
        {
            if((r.InTime > r.OutTime) || CheckConflictingBooking(r))
                return null;

            r.Id = _reservations.Max(x => x.Id) + 1;
            System.Console.WriteLine(r.Id);
            _reservations.Add(r);
            return (int) r.Id;
        }

        public bool CancelReservation(int reservationId)
        {
            var reservation = _reservations.FirstOrDefault(x => x.Id == reservationId);
            if(reservation != null)
            {
                _reservations.Remove(reservation);
                return true;
            }                
            else 
                return false;
        }
         

        public bool UpdateReservation(UpdateReservation r) 
        {
            var reservation = _reservations.FirstOrDefault(x => x.Id == r.Id);
            if(reservation == null || (r.InTime > r.OutTime) || CheckConflictingBooking(r))
                return false;
            reservation.InTime = r.InTime;
            reservation.OutTime = r.OutTime;
            return true;
        }


        public void AddReview(string review) 
        {
            // todo
        }
    }
}
