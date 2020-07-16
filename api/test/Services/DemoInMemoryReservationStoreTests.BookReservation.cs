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

            // same time but at a different address
            var booking2 = CreateTestReservation(addressId: 2);
            (result, id) = reservationStore.BookReservation(booking2);
            result.ShouldBe(EnumReservationResult.Success);
            id.ShouldBe(4);
        }

        [Fact]
        public void Book_EmptyBooking_Fail()
        {
            var booking = new ReservationCreate();
            var (result, id) = reservationStore.BookReservation(booking);
            result.ShouldBe(EnumReservationResult.BookingInvalid);
        }

        [Fact]
        public void Book_MaxBookingsExceeded_Fail()
        {
            for (int i = 0; i < 98; i++)
            {
                var (r, _) = reservationStore.BookReservation(CreateTestReservation(inTime: new DateTime(1995 + i, 1, 1), outTime: new DateTime(1995 + i, 2, 1)));
                r.ShouldBe(EnumReservationResult.Success);
            }
            var booking = CreateTestReservation(inTime: new DateTime(3000, 1, 1, 7, 0, 0), outTime: new DateTime(3000, 2, 1, 7, 0, 0));
            var (result, id) = reservationStore.BookReservation(booking);
            result.ShouldBe(EnumReservationResult.MaxBookingExceeded);
        }

        [Fact]
        public void Book_BookingTimesInvalid_Fail()
        {
            // inTime > outTime
            var booking = CreateTestReservation(inTime: new DateTime(3000, 1, 1, 7, 0, 0), outTime: new DateTime(2999, 2, 1, 7, 0, 0));
            var (result, id) = reservationStore.BookReservation(booking);
            result.ShouldBe(EnumReservationResult.BookingInvalid);
        }

        [Fact]
        public void Book_BookingConflict_Fail()
        {
            reservationStore.BookReservation(CreateTestReservation(inTime: new DateTime(3000, 1, 1), outTime: new DateTime(3000, 2, 1)));
            // same address id
            var booking2 = CreateTestReservation(inTime: new DateTime(3000, 1, 1, 7, 1, 0), outTime: new DateTime(3000, 1, 2, 7, 0, 0));
            var (result, id) = reservationStore.BookReservation(booking2);
            result.ShouldBe(EnumReservationResult.BookingConflict);
        }
    }
}