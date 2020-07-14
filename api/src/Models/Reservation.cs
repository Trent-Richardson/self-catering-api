using System;

namespace SelfCatering 
{
    public class AddressDetails 
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }

    public class ReservationCreate 
    {
        public AddressDetails Address { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string Review { get; set; }
    }
    public class Reservation : ReservationCreate
    {
        public Reservation() { }
        public Reservation(ReservationCreate r)
        {
            Address = r.Address;
            InTime = r.InTime;
            OutTime = r.OutTime;
        }

        public int? Id { get; set; }
    }
}

