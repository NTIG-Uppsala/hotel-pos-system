using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    [Collection("Requires Database")]
    public class BookingFormTests : IDisposable {
        private readonly UIA3Automation _automation;
        private readonly ProgramWithTestDatabase _programWithDatabase;

        public BookingFormTests() {
            _automation = new UIA3Automation();
            _programWithDatabase = new ProgramWithTestDatabase();
        }

        [Fact(Skip = "FlaUI error when selecting combobox item")]
        public void ShouldAddBooking() {
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
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

            TextBox startEndDatesLabel = Utilities.GetElement(_automation, "dates4", bookingList).AsTextBox();
            TextBox customerName = Utilities.GetElement(_automation, "customerName4", bookingList).AsTextBox();
            Assert.Contains("2024-01-25", startEndDatesLabel.Text);
            Assert.Contains("Kalle Kallesson", customerName.Text);
        }

        [Fact]
        public void ShouldDisplayErrorWhenEndDateBeforeStartDate() {
            AutomationElement bookingForm = GetBookingFormElement();
            DateTimePicker startDatePicker = Utilities.GetElement(_automation, "startDate", bookingForm).AsDateTimePicker();
            DateTimePicker endDatePicker = Utilities.GetElement(_automation, "endDate", bookingForm).AsDateTimePicker();
            Button addBookingButton = Utilities.GetElement(_automation, "addBooking", bookingForm).AsButton();

            startDatePicker.SelectedDate = new DateTime(2024, 2, 5);
            endDatePicker.SelectedDate = new DateTime(2024, 2, 3);
            addBookingButton.Click();

            // Get label when label is not empty, FlaUI probably can not find empty labels
            TextBox dateErrorLabel = Utilities.GetElement(_automation, "dateError", bookingForm).AsTextBox();
            Assert.Contains("end date is before start date", dateErrorLabel.Text.ToLower());
        }

        [Fact]
        public void ShouldNotAddBookingWhenNothingSelected() {
            AutomationElement bookingForm = GetBookingFormElement();
            Button addBookingButton = Utilities.GetElement(_automation, "addBooking", bookingForm).AsButton();
            AutomationElement bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);

            int bookingsBefore = bookingList.FindAllChildren().Count();
            addBookingButton.Click();

            bookingList = Utilities.GetBookingListElement(_programWithDatabase, _automation);
            int bookingsAfter = bookingList.FindAllChildren().Count();
            Assert.Equal(bookingsBefore, bookingsAfter);
        }

        [Fact]
        public void ShouldDisplayErrorMessageWhenNoCustomerSelected() {
            AutomationElement bookingForm = GetBookingFormElement();
            Button addBookingButton = Utilities.GetElement(_automation, "addBooking", bookingForm).AsButton();

            addBookingButton.Click();

            // Get label when label is not empty, FlaUI probably can not find empty labels
            TextBox customerErrorLabel = Utilities.GetElement(_automation, "customerError", bookingForm).AsTextBox();
            Assert.Equal("please select a customer", customerErrorLabel.Text.ToLower());
        }

        private AutomationElement GetBookingFormElement() {
            const string bookingListAutomationId = "bookingForm";
            return Utilities.GetElement(_automation, bookingListAutomationId, Utilities.GetMainWindow(_programWithDatabase, _automation));
        }

        public void Dispose() {
            _automation.Dispose();
            _programWithDatabase.Dispose();
        }
    }
}
