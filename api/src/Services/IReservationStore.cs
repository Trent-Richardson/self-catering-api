using System;
using System.Collections.Generic;
using Enums;

namespace SelfCatering 
{
    public interface IReservationStore
    {
        List<Reservation> GetReservations();
        Tuple<EnumReservationResult, int?> BookReservation(ReservationCreate r);
        EnumReservationResult CancelReservation(int reservationId);
        EnumReservationResult UpdateReservation(UpdateReservation r);
        EnumReservationResult AddReview(int id, string review);
    }
}
