using System;

namespace SelfCatering 
{
    public class AddressDetails 
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }
    public class Reservation
    {
        public int? Id { get; set; }
        public AddressDetails Address { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string Review { get; set; }
    }
}

