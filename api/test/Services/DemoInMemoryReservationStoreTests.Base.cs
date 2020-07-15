using SelfCatering;

namespace Services
{
    public class DemoInMemoryReservationStoreTestsBase
    {
        protected DemoInMemoryReservationStore reservationStore;

        public DemoInMemoryReservationStoreTestsBase()
        {
            reservationStore = new DemoInMemoryReservationStore();
        }
    }
}