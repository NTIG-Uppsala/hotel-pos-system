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
            Label customerName = GetElement("customerName1", bookingList).AsLabel();

            Assert.Contains("Robert Robertsson", customerName.Text);
        }

        [Fact]
        public void EmailAdressShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label emailAddress = GetElement("emailAddress2", bookingList).AsLabel();

            Assert.Contains("robert.robertsson@example.com", emailAddress.Text);
        }

        [Fact]
        public void PhoneNumberShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label phoneNumber = GetElement("phoneNumber3", bookingList).AsLabel();

            Assert.Contains("070-1740640", phoneNumber.Text);
        }

        [Fact]
        public void RoomNameShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label roomName = GetElement("roomName1", bookingList).AsLabel();

            Assert.Contains("202", roomName.Text);
        }

        [Fact]
        public void DatesShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label startEndDatesLabel = GetElement("dates2", bookingList).AsLabel();

            Assert.Contains("2023-12-27", startEndDatesLabel.Text);
            Assert.Contains("2024-01-03", startEndDatesLabel.Text);
        }

        [Fact]
        public void CheckedInStatusShouldBeCorrect() {
            AutomationElement bookingList = GetBookingListElement();
            CheckBox checkedInCheckBox = GetElement("checkedIn1", bookingList).AsCheckBox();
            CheckBox notCheckedInCheckBox = GetElement("checkedIn2", bookingList).AsCheckBox();

            Assert.True(checkedInCheckBox.IsChecked);
            Assert.False(notCheckedInCheckBox.IsChecked);
        }

        [Fact]
        public void PaidStatusShouldBeCorrect() {
            AutomationElement bookingList = GetBookingListElement();
            CheckBox paidCheckBox = GetElement("paidFor2", bookingList).AsCheckBox();
            CheckBox notPaidCheckBox = GetElement("paidFor1", bookingList).AsCheckBox();

            Assert.True(paidCheckBox.IsChecked);
            Assert.False(notPaidCheckBox.IsChecked);
        }

        [Fact]
        public void CommentShouldBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            Label commentLabel = GetElement("comment1", bookingList).AsLabel();

            Assert.Contains("Cleaning crew one hour late", commentLabel.Text);
        }

        [Fact]
        public void CommentShouldNotBePresentInList() {
            AutomationElement bookingList = GetBookingListElement();
            AutomationElement? commentLabel = GetElement("comment2", bookingList);

            Assert.Null(commentLabel);
        }

        /// <summary>
        /// Finds and returns an element with a specific <paramref name="automationId"/> in the <paramref name="parent"/> element
        /// </summary>
        /// <param name="automationId">The same as <see cref="System.Windows.Forms.Control.Name"/></param>
        /// <param name="parent">The element to start the search from</param>
        /// <returns>The element if found, otherwise <c>null</c></returns>
        private AutomationElement GetElement(string automationId, AutomationElement parent) {
            ConditionFactory conditionFactory = _automation.ConditionFactory;
            return parent.FindFirstDescendant(conditionFactory.ByAutomationId(automationId));
        }

        private AutomationElement GetBookingListElement() {
            const string bookingListAutomationId = "bookingList";
            return GetElement(bookingListAutomationId, GetMainWindow());
        }

        private Window GetMainWindow() {
            return _fixture.Application.GetMainWindow(_automation).AsWindow();
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
