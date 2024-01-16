using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    [Collection("Requires Database")]
    public class BookingFormTests : IDisposable, IClassFixture<ProgramFixture> {
        private readonly UIA3Automation _automation;
        private readonly ProgramFixture _fixture;

        public BookingFormTests(ProgramFixture fixture) {
            _automation = new UIA3Automation();
            _fixture = fixture;
        }

        [Fact(Skip = "FlaUI error when selecting combobox item")]
        public void ShouldAddBooking() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_fixture, _automation);
            AutomationElement bookingForm = GetBookingFormElement();
            ComboBox customerDropdown = Utilities.GetElement(_automation, "customer", bookingForm).AsComboBox();
            DateTimePicker startDatePicker = Utilities.GetElement(_automation, "startDate", bookingForm).AsDateTimePicker();
            DateTimePicker endDatePicker = Utilities.GetElement(_automation, "endDate", bookingForm).AsDateTimePicker();
            ComboBox roomDropdown = Utilities.GetElement(_automation, "room", bookingForm).AsComboBox();
            TextBox commentTextBox = Utilities.GetElement(_automation, "comment", bookingForm).AsTextBox();
            CheckBox paidCheckBox = Utilities.GetElement(_automation, "paidFor", bookingForm).AsCheckBox();
            Button addBookingButton = Utilities.GetElement(_automation, "addBooking", bookingForm).AsButton();

            customerDropdown.Select("Kalle Kallesson");
            startDatePicker.SelectedDate = new DateTime(2024, 1, 25);
            endDatePicker.SelectedDate = new DateTime(2024, 2, 3);
            roomDropdown.Select("202");
            commentTextBox.Text = "Arrives late";
            paidCheckBox.Click();
            addBookingButton.Click();

            Label startEndDatesLabel = Utilities.GetElement(_automation, "dates4", bookingList).AsLabel();
            Label customerName = Utilities.GetElement(_automation, "customerName4", bookingList).AsLabel();
            Assert.Contains("2024-01-25", startEndDatesLabel.Text);
            Assert.Contains("Kalle Kallesson", customerName.Text);
        }

        private AutomationElement GetBookingFormElement() {
            const string bookingListAutomationId = "bookingForm";
            return Utilities.GetElement(_automation, bookingListAutomationId, Utilities.GetMainWindow(_fixture, _automation));
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
