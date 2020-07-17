using System;
using SelfCatering;

namespace Services
{
    public class DemoInMemoryReservationStoreTestsBase
    {
        protected DemoInMemoryReservationStore reservationStore;

        protected DemoInMemoryReservationStoreTestsBase()
        {
            reservationStore = new DemoInMemoryReservationStore();
        }

        protected void InitializeTestData()
        {
            reservationStore.BookReservation(new ReservationCreate
            {
                Address = new AddressDetails 
                    {
                        Id = 1,
                        Address = "12 Barbados St"
                    },
                InTime = new DateTime(1990,1,1),
                OutTime = new DateTime(1990,1,5)
            });
            reservationStore.BookReservation(new ReservationCreate
            {
                Address = new AddressDetails 
                    {
                        Id = 1,
                        Address = "12 Barbados St"
                    },
                InTime = new DateTime(1991,1,1),
                OutTime = new DateTime(1991,1,5)
            });
        }
    }
}