using Enums;
using Xunit;
using Shouldly;

namespace Services
{
    /// <summary>
    /// booking cancelled - success
    /// booking not found - fail
    /// </summary>
    public class DemoInMemoryReservationStoreTestsCancelReservation : DemoInMemoryReservationStoreTestsBase
    {
        [Fact]
        public void CancelBooking_BookingFound_Success()
        {
            InitializeTestData();
            var result = reservationStore.CancelReservation(1);
            result.ShouldBe(EnumReservationResult.Success);
            reservationStore.GetReservations().Count.ShouldBe(1);
        }

        [Fact]
        public void CancelBooking_BookingNotFound_Fail()
        {
            var result = reservationStore.CancelReservation(99);
            result.ShouldBe(EnumReservationResult.NotFound);
        }
    }
}