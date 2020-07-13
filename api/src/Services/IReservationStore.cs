using System.Collections.Generic;

namespace SelfCatering 
{
    public interface IReservationStore
    {
        List<Reservation> GetReservations();
        int? BookReservation(Reservation r);
        bool CancelReservation(int reservationId);
        bool UpdateReservation(UpdateReservation r);
        bool AddReview(int id, string review);
    }
}
