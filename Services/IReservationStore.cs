using System.Collections.Generic;
using System;
using System.Linq;


namespace SelfCatering 
{
    public interface IReservationStore
    {
        List<Reservation> GetReservations();
        int? BookReservation(Reservation r);
        void CancelReservation(int reservationId);
        void UpdateReservation(DateTime inTime, DateTime outTime);
        void AddReview(string review);
    }
}
