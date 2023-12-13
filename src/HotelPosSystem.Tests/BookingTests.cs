using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    public class BookingTests : IDisposable, IClassFixture<ProgramFixture> {
        private readonly UIA3Automation _automation;
        private readonly ProgramFixture _fixture;

        private const string BookingListAutomationId = "bookingList";

        public BookingTests(ProgramFixture fixture) {
            _automation = new UIA3Automation();
            _fixture = fixture;
        }

        [Fact]
        public void CustomerNameShouldBePresentInList() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            Label customerName = GetElement("customerName1", bookingList).AsLabel();

            Assert.Contains("Robert Robertsson", customerName.Text);
        }

        [Fact]
        public void EmailAdressShouldBePresentInList() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            Label emailAddress = GetElement("emailAddress2", bookingList).AsLabel();

            Assert.Contains("robert.robertsson@example.com", emailAddress.Text);
        }

        [Fact]
        public void PhoneNumberShouldBePresentInList() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            Label phoneNumber = GetElement("phoneNumber3", bookingList).AsLabel();

            Assert.Contains("070-1740640", phoneNumber.Text);
        }

        [Fact]
        public void RoomNameShouldBePresentInList() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            Label roomName = GetElement("roomName1", bookingList).AsLabel();

            Assert.Contains("202", roomName.Text);
        }

        [Fact]
        public void DatesShouldBePresentInList() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            Label startEndDatesLabel = GetElement("dates2", bookingList).AsLabel();

            Assert.Contains("2023-12-27", startEndDatesLabel.Text);
            Assert.Contains("2024-01-03", startEndDatesLabel.Text);
        }

        [Fact]
        public void CheckedInStatusShouldBeCorrect() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            CheckBox checkedInCheckBox = GetElement("checkedIn1", bookingList).AsCheckBox();
            CheckBox notCheckedInCheckBox = GetElement("checkedIn2", bookingList).AsCheckBox();

            Assert.True(checkedInCheckBox.IsChecked);
            Assert.False(notCheckedInCheckBox.IsChecked);
        }

        [Fact]
        public void PaidStatusShouldBeCorrect() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            CheckBox paidCheckBox = GetElement("paidFor2", bookingList).AsCheckBox();
            CheckBox notPaidCheckBox = GetElement("paidFor1", bookingList).AsCheckBox();

            Assert.True(paidCheckBox.IsChecked);
            Assert.False(notPaidCheckBox.IsChecked);
        }

        [Fact]
        public void CommentShouldBePresentInList() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            Label commentLabel = GetElement("comment1", bookingList).AsLabel();

            Assert.Contains("Cleaning crew one hour late", commentLabel.Text);
        }

        [Fact]
        public void CommentShouldNotBePresentInList() {
            AutomationElement bookingList = GetElement(BookingListAutomationId, GetMainWindow());
            AutomationElement? commentLabel = GetElement("comment2", bookingList);

            Assert.Null(commentLabel);
        }

        private AutomationElement GetElement(string automationId, AutomationElement parent) {
            ConditionFactory conditionFactory = _automation.ConditionFactory;
            return parent.FindFirstDescendant(conditionFactory.ByAutomationId(automationId));
        }

        private Window GetMainWindow() {
            return _fixture.Application.GetMainWindow(_automation).AsWindow();
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
