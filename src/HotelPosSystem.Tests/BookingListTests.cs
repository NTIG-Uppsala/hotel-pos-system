using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    public class BookingListTests : IDisposable, IClassFixture<ProgramFixture> {
        private readonly UIA3Automation _automation;
        private readonly ProgramFixture _fixture;

        public BookingListTests(ProgramFixture fixture) {
            _automation = new UIA3Automation();
            _fixture = fixture;
        }

        [Fact]
        public void CustomerNameShouldBePresentInList() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            Label customerName = TestUtilities.GetElement("customerName1", bookingList, _automation).AsLabel();

            Assert.Contains("Robert Robertsson", customerName.Text);
        }

        [Fact]
        public void EmailAdressShouldBePresentInList() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            Label emailAddress = TestUtilities.GetElement("emailAddress2", bookingList, _automation).AsLabel();

            Assert.Contains("robert.robertsson@example.com", emailAddress.Text);
        }

        [Fact]
        public void PhoneNumberShouldBePresentInList() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            Label phoneNumber = TestUtilities.GetElement("phoneNumber3", bookingList, _automation).AsLabel();

            Assert.Contains("070-1740640", phoneNumber.Text);
        }

        [Fact]
        public void RoomNameShouldBePresentInList() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            Label roomName = TestUtilities.GetElement("roomName1", bookingList, _automation).AsLabel();

            Assert.Contains("202", roomName.Text);
        }

        [Fact]
        public void DatesShouldBePresentInList() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            Label startEndDatesLabel = TestUtilities.GetElement("dates2", bookingList, _automation).AsLabel();

            Assert.Contains("2023-12-27", startEndDatesLabel.Text);
            Assert.Contains("2024-01-03", startEndDatesLabel.Text);
        }

        [Fact]
        public void CheckedInStatusShouldBeCorrect() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            CheckBox checkedInCheckBox = TestUtilities.GetElement("checkedIn1", bookingList, _automation).AsCheckBox();
            CheckBox notCheckedInCheckBox = TestUtilities.GetElement("checkedIn2", bookingList, _automation).AsCheckBox();

            Assert.True(checkedInCheckBox.IsChecked);
            Assert.False(notCheckedInCheckBox.IsChecked);
        }

        [Fact]
        public void PaidStatusShouldBeCorrect() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            CheckBox paidCheckBox = TestUtilities.GetElement("paidFor2", bookingList, _automation).AsCheckBox();
            CheckBox notPaidCheckBox = TestUtilities.GetElement("paidFor1", bookingList, _automation).AsCheckBox();

            Assert.True(paidCheckBox.IsChecked);
            Assert.False(notPaidCheckBox.IsChecked);
        }

        [Fact]
        public void CommentShouldBePresentInList() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            Label commentLabel = TestUtilities.GetElement("comment1", bookingList, _automation).AsLabel();

            Assert.Contains("Cleaning crew one hour late", commentLabel.Text);
        }

        [Fact]
        public void CommentShouldNotBePresentInList() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);
            AutomationElement? commentLabel = TestUtilities.GetElement("comment2", bookingList, _automation);

            Assert.Null(commentLabel);
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
