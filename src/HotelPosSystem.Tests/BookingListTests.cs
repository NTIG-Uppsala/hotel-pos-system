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
            AutomationElement bookingList = GetBookingListElement();
            Label customerName = Utilities.GetElement(_automation, "customerName1", bookingList).AsLabel();

            Assert.Contains("Robert Robertsson", customerName.Text);
        }

        [Fact]
        public void EmailAdressShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label emailAddress = Utilities.GetElement(_automation, "emailAddress2", bookingList).AsLabel();

            Assert.Contains("robert.robertsson@example.com", emailAddress.Text);
        }

        [Fact]
        public void PhoneNumberShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label phoneNumber = Utilities.GetElement(_automation, "phoneNumber3", bookingList).AsLabel();

            Assert.Contains("070-1740640", phoneNumber.Text);
        }

        [Fact]
        public void RoomNameShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label roomName = Utilities.GetElement(_automation, "roomName1", bookingList).AsLabel();

            Assert.Contains("202", roomName.Text);
        }

        [Fact]
        public void DatesShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label startEndDatesLabel = Utilities.GetElement(_automation, "dates2", bookingList).AsLabel();

            Assert.Contains("2023-12-27", startEndDatesLabel.Text);
            Assert.Contains("2024-01-03", startEndDatesLabel.Text);
        }

        [Fact]
        public void CheckedInStatusShouldBeCorrect() {
            AutomationElement bookingList = GetBookingListElement();
            CheckBox checkedInCheckBox = Utilities.GetElement(_automation, "checkedIn1", bookingList).AsCheckBox();
            CheckBox notCheckedInCheckBox = Utilities.GetElement(_automation, "checkedIn2", bookingList).AsCheckBox();

            Assert.True(checkedInCheckBox.IsChecked);
            Assert.False(notCheckedInCheckBox.IsChecked);
        }

        [Fact]
        public void PaidStatusShouldBeCorrect() {
            AutomationElement bookingList = GetBookingListElement();
            CheckBox paidCheckBox = Utilities.GetElement(_automation, "paidFor2", bookingList).AsCheckBox();
            CheckBox notPaidCheckBox = Utilities.GetElement(_automation, "paidFor1", bookingList).AsCheckBox();

            Assert.True(paidCheckBox.IsChecked);
            Assert.False(notPaidCheckBox.IsChecked);
        }

        [Fact]
        public void CommentShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label commentLabel = Utilities.GetElement(_automation, "comment1", bookingList).AsLabel();

            Assert.Contains("Cleaning crew one hour late", commentLabel.Text);
        }

        [Fact]
        public void CommentShouldNotBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            AutomationElement? commentLabel = Utilities.GetElement(_automation, "comment2", bookingList);

            Assert.Null(commentLabel);
        }

        private AutomationElement GetBookingListElement() {
            const string bookingListAutomationId = "bookingList";
            return Utilities.GetElement(_automation, bookingListAutomationId, Utilities.GetMainWindow(_fixture, _automation));
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
