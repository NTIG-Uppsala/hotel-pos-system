using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    [Collection("Requires Database")]
    public class BookingListTests : IDisposable {
        private readonly UIA3Automation _automation;
        private readonly ProgramWithTestDatabase _programWithDatabase;

        public BookingListTests() {
            _automation = new UIA3Automation();
            _programWithDatabase = new ProgramWithTestDatabase();
        }

        [Fact]
        public void CustomerNameShouldBeInList() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            Label customerName = Utilities.GetElement(_automation, "customerName1", bookingList).AsLabel();

            Assert.Contains("Robert Robertsson", customerName.Text);
        }

        [Fact]
        public void EmailAddressShouldBeInList() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            Label emailAddress = Utilities.GetElement(_automation, "emailAddress2", bookingList).AsLabel();

            Assert.Contains("robert.robertsson@example.com", emailAddress.Text);
        }

        [Fact]
        public void PhoneNumberShouldBeInList() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            Label phoneNumber = Utilities.GetElement(_automation, "phoneNumber3", bookingList).AsLabel();

            Assert.Contains("070-1740640", phoneNumber.Text);
        }

        [Fact]
        public void RoomNameShouldBeInList() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            Label roomName = Utilities.GetElement(_automation, "roomName1", bookingList).AsLabel();

            Assert.Contains("202", roomName.Text);
        }

        [Fact]
        public void DatesShouldBeInList() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            Label startEndDatesLabel = Utilities.GetElement(_automation, "dates2", bookingList).AsLabel();

            Assert.Contains("2023-12-27", startEndDatesLabel.Text);
            Assert.Contains("2024-01-03", startEndDatesLabel.Text);
        }

        [Fact]
        public void CheckedInStatusShouldBeCorrect() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            CheckBox checkedInCheckBox = Utilities.GetElement(_automation, "checkedIn1", bookingList).AsCheckBox();
            CheckBox notCheckedInCheckBox = Utilities.GetElement(_automation, "checkedIn2", bookingList).AsCheckBox();

            Assert.True(checkedInCheckBox.IsChecked);
            Assert.False(notCheckedInCheckBox.IsChecked);
        }

        [Fact]
        public void PaidStatusShouldBeCorrect() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            CheckBox paidCheckBox = Utilities.GetElement(_automation, "paidFor2", bookingList).AsCheckBox();
            CheckBox notPaidCheckBox = Utilities.GetElement(_automation, "paidFor1", bookingList).AsCheckBox();

            Assert.True(paidCheckBox.IsChecked);
            Assert.False(notPaidCheckBox.IsChecked);
        }

        [Fact]
        public void CommentShouldBeInList() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            Label commentLabel = Utilities.GetElement(_automation, "comment1", bookingList).AsLabel();

            Assert.Contains("Cleaning crew one hour late", commentLabel.Text);
        }

        [Fact]
        public void CommentShouldNotBeInList() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            AutomationElement? commentLabel = Utilities.GetElement(_automation, "comment2", bookingList);

            Assert.Null(commentLabel);
        }

        [Fact]
        public void BookingShouldBeRemoved() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            Button removeButton = Utilities.GetElement(_automation, "removeButton2", bookingList).AsButton();

            removeButton.Click();
            Window modalWindow = _programWithDatabase.Application.GetAllTopLevelWindows(_automation)[0];

            // AutomationId explanation: https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.dialogresult?view=windowsdesktop-8.0#fields
            Button yesButton = Utilities.GetElement(_automation, "6", modalWindow).AsButton();

            yesButton.Click();

            Label? customerName = Utilities.GetElement(_automation, "customerName2", bookingList).AsLabel();
            Assert.Null(customerName);
        }

        [Fact]
        public void BookingShouldNotBeRemoved() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            Button removeButton = Utilities.GetElement(_automation, "removeButton2", bookingList).AsButton();

            removeButton.Click();
            Window modalWindow = _programWithDatabase.Application.GetAllTopLevelWindows(_automation)[0];

            // AutomationId explanation: https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.dialogresult?view=windowsdesktop-8.0#fields
            Button noButton = Utilities.GetElement(_automation, "7", modalWindow).AsButton();

            noButton.Click();

            Label? customerName = Utilities.GetElement(_automation, "customerName2", bookingList).AsLabel();
            Assert.NotNull(customerName);
        }

        public void Dispose() {
            _automation.Dispose();
            _programWithDatabase.Dispose();
        }
    }
}
