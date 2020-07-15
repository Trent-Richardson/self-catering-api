using System;
using Enums;
using SelfCatering;
using Xunit;
using Shouldly;

namespace Services
{
    /// <summary>
    /// - successful booking
    /// - null booking - fail
    /// - max bookings exceeded - fail
    /// - inTime > outTime - fail
    /// - booking conflict - fail
    /// </summary>
    public class DemoInMemoryReservationStoreTestsBookReservation : DemoInMemoryReservationStoreTestsBase
    {
        
        private ReservationCreate CreateTestReservation(string address = "12 Parkway Drive", int addressId = 1, DateTime? inTime = null,  DateTime? outTime = null) 
            => new ReservationCreate
                {
                        Address = new AddressDetails
                        {
                            Address = address,
                            Id = addressId
                        },
                        InTime = inTime ?? new DateTime(1995, 1, 1, 7, 0, 0),
                        OutTime = outTime ?? new DateTime(1995, 1, 1, 12, 0, 0)
                };

        [Fact]
        public void Book_NoConflicts_Successful()
        {
            var booking = CreateTestReservation();
            var (result, id) = reservationStore.BookReservation(booking);
            result.ShouldBe(EnumReservationResult.Success);
            id.ShouldBe(3);
        }

        [Fact]
        public void Book_EmptyBooking_Fail()
        {
            var booking = new ReservationCreate();
            var (result, id) = reservationStore.BookReservation(booking);
            result.ShouldBe(EnumReservationResult.BookingInvalid);
        }

    }
}