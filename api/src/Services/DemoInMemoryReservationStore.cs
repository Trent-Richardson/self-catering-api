using System.Collections.Generic;
using System.Linq;
using System;
using Enums;
using Constants;

namespace SelfCatering 
{
    public class DemoInMemoryReservationStore : IReservationStore
    {
        private List<Reservation> _reservations = new List<Reservation>();

        public DemoInMemoryReservationStore()
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

        private bool CheckConflictingBooking(Reservation r) => _reservations.Any(x => x.Address.Id == r.Address.Id 
                                                                                    && (r.InTime >= x.InTime && r.InTime <= x.OutTime) 
                                                                                    && (r.OutTime >= x.InTime && r.OutTime <= x.OutTime));

        public List<Reservation> GetReservations() => _reservations;
        
        public Tuple<EnumReservationResult, int?> BookReservation(ReservationCreate r) 
        {      
            if(_reservations.Count >= ConstantsReservation.MAX_BOOKINGS)
                return new Tuple<EnumReservationResult, int?>(EnumReservationResult.MaxBookingExceeded, null);      
                
            var reservation = new Reservation(r); 

            if((r.InTime > r.OutTime) || CheckConflictingBooking(reservation))
                return new Tuple<EnumReservationResult, int?>(EnumReservationResult.BookingConflict, null);

            reservation.Id = _reservations.Max(x => x.Id) + 1;
            _reservations.Add(reservation);
            return new Tuple<EnumReservationResult, int?>(EnumReservationResult.Success, (int) reservation.Id);
        }

        public EnumReservationResult CancelReservation(int reservationId)
        {
            var reservation = _reservations.FirstOrDefault(x => x.Id == reservationId);
            if(reservation != null)
            {
                _reservations.Remove(reservation);
                return EnumReservationResult.Success;
            }                
            else 
                return EnumReservationResult.NotFound;
        }
         

        public EnumReservationResult UpdateReservation(UpdateReservation r) 
        {
            var reservation = _reservations.FirstOrDefault(x => x.Id == r.Id);
            if(reservation == null) 
                return EnumReservationResult.NotFound;
            if((r.InTime > r.OutTime) 
                    || CheckConflictingBooking(new Reservation{ Address = reservation.Address, InTime = r.InTime, OutTime = r.OutTime }))
                return EnumReservationResult.BookingConflict;
            reservation.InTime = r.InTime;
            reservation.OutTime = r.OutTime;
            return EnumReservationResult.Success;
        }

        public EnumReservationResult AddReview(int id, string review)
        {
            if(review.Length > ConstantsReservation.MAX_REVIEW_LENGTH)
                return EnumReservationResult.MaxReviewLengthExceeded;
            var reservation = _reservations.FirstOrDefault(x => x.Id == id);
            if(reservation == null)
                return EnumReservationResult.NotFound;
            reservation.Review = review;
            return EnumReservationResult.Success;
        }
    }
}
