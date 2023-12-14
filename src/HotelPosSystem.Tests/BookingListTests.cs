using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
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
            Label customerName = TestUtilities.GetElement("customerName1", bookingList, _automation).AsLabel();

            Assert.Contains("Robert Robertsson", customerName.Text);
        }

        [Fact]
        public void EmailAdressShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label emailAddress = TestUtilities.GetElement("emailAddress2", bookingList, _automation).AsLabel();

            Assert.Contains("robert.robertsson@example.com", emailAddress.Text);
        }

        [Fact]
        public void PhoneNumberShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label phoneNumber = TestUtilities.GetElement("phoneNumber3", bookingList, _automation).AsLabel();

            Assert.Contains("070-1740640", phoneNumber.Text);
        }

        [Fact]
        public void RoomNameShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label roomName = TestUtilities.GetElement("roomName1", bookingList, _automation).AsLabel();

            Assert.Contains("202", roomName.Text);
        }

        [Fact]
        public void DatesShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label startEndDatesLabel = TestUtilities.GetElement("dates2", bookingList, _automation).AsLabel();

            Assert.Contains("2023-12-27", startEndDatesLabel.Text);
            Assert.Contains("2024-01-03", startEndDatesLabel.Text);
        }

        [Fact]
        public void CheckedInStatusShouldBeCorrect() {
            AutomationElement bookingList = GetBookingListElement();
            CheckBox checkedInCheckBox = TestUtilities.GetElement("checkedIn1", bookingList, _automation).AsCheckBox();
            CheckBox notCheckedInCheckBox = TestUtilities.GetElement("checkedIn2", bookingList, _automation).AsCheckBox();

            Assert.True(checkedInCheckBox.IsChecked);
            Assert.False(notCheckedInCheckBox.IsChecked);
        }

        [Fact]
        public void PaidStatusShouldBeCorrect() {
            AutomationElement bookingList = GetBookingListElement();
            CheckBox paidCheckBox = TestUtilities.GetElement("paidFor2", bookingList, _automation).AsCheckBox();
            CheckBox notPaidCheckBox = TestUtilities.GetElement("paidFor1", bookingList, _automation).AsCheckBox();

            Assert.True(paidCheckBox.IsChecked);
            Assert.False(notPaidCheckBox.IsChecked);
        }

        [Fact]
        public void CommentShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label commentLabel = TestUtilities.GetElement("comment1", bookingList, _automation).AsLabel();

            Assert.Contains("Cleaning crew one hour late", commentLabel.Text);
        }

        [Fact]
        public void CommentShouldNotBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            AutomationElement? commentLabel = TestUtilities.GetElement("comment2", bookingList, _automation);

            Assert.Null(commentLabel);
        }

        private AutomationElement GetBookingListElement() {
            const string bookingListAutomationId = "bookingList";
            return TestUtilities.GetElement(bookingListAutomationId, TestUtilities.GetMainWindow(_fixture, _automation), _automation);
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
