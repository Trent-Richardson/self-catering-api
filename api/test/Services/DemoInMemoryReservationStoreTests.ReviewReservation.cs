using Enums;
using Xunit;
using Shouldly;
using SelfCatering;
using System;
using System.Linq;

namespace Services
{
    /// <summary>
    /// valid review - success
    /// reservation not found - fail
    /// max review length exceeded - fail
    /// </summary>
    public class DemoInMemoryReservationStoreTestsReviewReservation : DemoInMemoryReservationStoreTestsBase
    {
        [Fact]
        public void ReviewBooking_ValidReview_Success()
        {
            var review = "lovely atmosphere!";
            var result = reservationStore.AddReview(1, review);
            result.ShouldBe(EnumReservationResult.Success);
            var booking = reservationStore.GetReservations().First(x => x.Id == 1);
            booking.Review.ShouldBe(review);
        }

        [Fact]
        public void ReviewBooking_BookingNotFound_Fail()
        {
            var result = reservationStore.AddReview(99, "not easy to find ...");
            result.ShouldBe(EnumReservationResult.NotFound);        
        }

        [Fact]
        public void ReviewBooking_ReviewLengthExceeded_Fail()
        {
            var result = reservationStore.AddReview(1, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            result.ShouldBe(EnumReservationResult.MaxReviewLengthExceeded);
        }
    }
}