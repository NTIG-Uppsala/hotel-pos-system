using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    public class AddBookingTests : IDisposable, IClassFixture<ProgramFixture> {
        private readonly UIA3Automation _automation;
        private readonly ProgramFixture _fixture;

        public AddBookingTests(ProgramFixture fixture) {
            _automation = new UIA3Automation();
            _fixture = fixture;
        }

        [Fact]
        public void ShouldAddBooking() {
            AutomationElement bookingList = TestUtilities.GetBookingListElement(_fixture, _automation);

            const string commentText = "Comment for testing";
            AddBooking("Robert Robertsson", new DateTime(2023, 5, 1), new DateTime(2023, 5, 5), "201", commentText, paidFor: true, checkedIn: false);

            Label? commentLabel = bookingList.FindFirstDescendant(descendant => descendant.ByText(commentText)).AsLabel();

            Assert.NotNull(commentLabel);
            Assert.Equal(commentText, commentLabel.Text);
        }

        private void AddBooking(string customerName, DateTime startDate, DateTime endDate, string roomName, string commentText, bool paidFor, bool checkedIn) {
            AutomationElement addBookingPanel = TestUtilities.GetAddBookingPanel(_fixture, _automation);

            ComboBox customerInput = TestUtilities.GetElement("customerInput", addBookingPanel, _automation).AsComboBox();
            DateTimePicker startDateInput = TestUtilities.GetElement("startDateInput", addBookingPanel, _automation).AsDateTimePicker();
            DateTimePicker endDateInput = TestUtilities.GetElement("endDateInput", addBookingPanel, _automation).AsDateTimePicker();
            ComboBox roomInput = TestUtilities.GetElement("roomInput", addBookingPanel, _automation).AsComboBox();
            TextBox commentInput = TestUtilities.GetElement("commentInput", addBookingPanel, _automation).AsTextBox();
            CheckBox hasPaidCheckBox = TestUtilities.GetElement("hasPaidCheckBox", addBookingPanel, _automation).AsCheckBox();
            CheckBox hasCheckedInCheckBox = TestUtilities.GetElement("hasCheckedInCheckBox", addBookingPanel, _automation).AsCheckBox();
            Button addBookingButton = TestUtilities.GetElement("addBookingButton", addBookingPanel, _automation).AsButton();

            customerInput.Select(customerName);
            startDateInput.SelectedDate = startDate;
            endDateInput.SelectedDate = endDate;
            roomInput.Select(roomName);
            commentInput.Text = commentText;
            hasPaidCheckBox.IsChecked = paidFor;
            hasCheckedInCheckBox.IsChecked = checkedIn;
            addBookingButton.Click();
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
