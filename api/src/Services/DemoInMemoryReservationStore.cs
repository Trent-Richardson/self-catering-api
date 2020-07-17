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

        private bool CheckValidBooking(ReservationCreate r) =>  r.Address != null 
                                                                && r.Address.Address != null
                                                                && r.Address.Address != ""
                                                                && r.Address.Id > 0
                                                                && r.InTime != null
                                                                && r.OutTime != null
                                                                && (r.InTime < r.OutTime)
                                                                && (r.OutTime.Subtract(r.InTime).TotalHours <= ConstantsReservation.MAX_BOOKING_LENGTH_HOURS);
                                                      
        private bool CheckConflictingBooking(Reservation r) => _reservations.Any(x => x.Address.Id == r.Address.Id 
                                                                                    && (CheckConflictingHours(r.InTime, r.OutTime, x.InTime, x.OutTime)));
        private bool CheckConflictingHours(DateTime newInTime, DateTime newOutTime, DateTime currentInTime, DateTime currentOutTime)
        {
            var newBookingRange = GenerateHourIncrements(newInTime, newOutTime);

            return newBookingRange.Any(x => currentInTime <= x && currentOutTime >= x);

            List<DateTime> GenerateHourIncrements(DateTime inTime, DateTime outTime) 
            {
                var increments = outTime.Subtract(inTime);
                var range = new List<DateTime>();
                for (int i = 0; i < increments.TotalHours; i++)
                {
                    range.Add(inTime.AddHours(i));
                }
                return range;
            } 
        }
        public List<Reservation> GetReservations() => _reservations;
        
        public Tuple<EnumReservationResult, int?> BookReservation(ReservationCreate r) 
        {      
            if(!CheckValidBooking(r))
                return new Tuple<EnumReservationResult, int?>(EnumReservationResult.BookingInvalid, null);      

            if(_reservations.Count >= ConstantsReservation.MAX_BOOKINGS)
                return new Tuple<EnumReservationResult, int?>(EnumReservationResult.MaxBookingExceeded, null);      
                
            // normalize booking times to hour
            var reservation = new Reservation(r);
            reservation.InTime = NormalizeBookingDate(r.InTime);
            reservation.OutTime = NormalizeBookingDate(r.OutTime);

            if(CheckConflictingBooking(reservation))
                return new Tuple<EnumReservationResult, int?>(EnumReservationResult.BookingConflict, null);

            reservation.Id = _reservations.Count == 0 ? 1 : _reservations.Max(x => x.Id) + 1;
            _reservations.Add(reservation);
            return new Tuple<EnumReservationResult, int?>(EnumReservationResult.Success, (int) reservation.Id);

            DateTime NormalizeBookingDate(DateTime bookingtime) => new DateTime(bookingtime.Year, bookingtime.Month, bookingtime.Day, bookingtime.Hour, 0, 0);

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

            var updatedReservation = new Reservation{ Address = reservation.Address, InTime = r.InTime, OutTime = r.OutTime };
            if(!CheckValidBooking(updatedReservation))
                return EnumReservationResult.BookingInvalid;

            if(CheckConflictingBooking(updatedReservation))
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
