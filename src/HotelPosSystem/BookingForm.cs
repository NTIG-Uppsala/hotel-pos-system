using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    internal class BookingForm {
        private readonly BookingList _bookingList;

        private ComboBox? _customerDropdown;
        private DateTimePicker? _startDatePicker;
        private DateTimePicker? _endDatePicker;
        private ComboBox? _roomDropdown;
        private TextBox? _commentTextBox;
        private CheckBox? _paidForCheckBox;
        private CheckBox? _checkedInCheckBox;

        internal FlowLayoutPanel ContainerPanel;

        internal BookingForm(BookingList bookingList) {
            _bookingList = bookingList;
            ContainerPanel = CreateFormControls();
        }

        private FlowLayoutPanel CreateFormControls() {
            using HotelDbContext databaseContext = new();

            FlowLayoutPanel formPanel = new() {
                Name = "bookingForm",
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Dock = DockStyle.Fill
            };

            Label heading = new() {
                Text = "Create new booking",
                Font = new Font(formPanel.Font.FontFamily, MainForm.HeadingFontSize),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, bottom: MainForm.MarginSize),
                UseCompatibleTextRendering = true
            };
            formPanel.Controls.Add(heading);

            const int width = 400 + MainForm.MarginSize;

            Customer[] customers = databaseContext.Customers
                .OrderBy(customer => customer.FullName)
                .ToArray();
            (_customerDropdown, _) = ControlUtilities.AddComboBoxWithLabel(formPanel, "customer", customers, width, "customerLabel", "Customer:");

            FlowLayoutPanel dateContainer = new() {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(0)
            };
            formPanel.Controls.Add(dateContainer);
            (_startDatePicker, _) = ControlUtilities.AddDatePickerWithLabel(dateContainer, "startDate", DateTime.Now, "startDateLabel", "Start date:");
            (_endDatePicker, _) = ControlUtilities.AddDatePickerWithLabel(dateContainer, "endDate", DateTime.Now, "endDateLabel", "End date:");
            _startDatePicker.Margin = new Padding(_startDatePicker.Margin.Left, _startDatePicker.Margin.Top, right: MainForm.MarginSize, _startDatePicker.Margin.Bottom);
            _endDatePicker.Margin = new Padding(left: 0, _endDatePicker.Margin.Top, _endDatePicker.Margin.Right, _endDatePicker.Margin.Bottom);

            Room[] rooms = databaseContext.Rooms
                .Include(room => room.Type)
                .OrderBy(room => room.Name)
                .ToArray();
            (_roomDropdown, _) = ControlUtilities.AddComboBoxWithLabel(formPanel, "room", rooms, width, "roomLabel", "Room:");

            (_commentTextBox, _) = ControlUtilities.AddTextBoxWithLabel(formPanel, "comment", width, "commentLabel", "Comment:");

            TableLayoutPanel checkBoxContainer = new() {
                RowCount = 1,
                ColumnCount = 2,
                Width = width,
                Margin = new Padding(0)
            };
            ControlUtilities.CreateRowsAndColumns(checkBoxContainer);
            formPanel.Controls.Add(checkBoxContainer);
            (_paidForCheckBox, _) = ControlUtilities.AddCheckBoxWithLabel(checkBoxContainer, "paidFor", checkBoxState: false, checkBoxEnabled: true, "paidForLabel", "Paid for:");
            (_checkedInCheckBox, Label checkedInLabel) = ControlUtilities.AddCheckBoxWithLabel(checkBoxContainer, "checkedIn", checkBoxState: false, checkBoxEnabled: true, "checkedInLabel", "Checked in:");
            checkedInLabel.Margin = new Padding(left: MainForm.MarginSize / 2, checkedInLabel.Margin.Top, checkedInLabel.Margin.Right, checkedInLabel.Margin.Bottom);

            ControlUtilities.AddButton(formPanel, "addBooking", "Add Booking", width, (sender, eventArgs) => CreateBooking());

            return formPanel;
        }

        private void CreateBooking() {
            using HotelDbContext databaseContext = new();

            Customer? customer = _customerDropdown?.SelectedValue as Customer;
            if (_startDatePicker is null || _endDatePicker is null) {
                throw new NullReferenceException();
            }
            DateOnly startDate = DateOnly.FromDateTime(_startDatePicker.Value);
            DateOnly endDate = DateOnly.FromDateTime(_endDatePicker.Value);
            Room? room = _roomDropdown?.SelectedValue as Room;
            string? comment = _commentTextBox?.Text;
            bool? paidFor = _paidForCheckBox?.Checked;
            bool? checkedIn = _checkedInCheckBox?.Checked;

            if (customer is null || room is null || paidFor is null || checkedIn is null) {
                throw new NullReferenceException();
            }

            Booking booking = new() {
                Customer = customer,
                StartDate = startDate,
                EndDate = endDate,
                Room = room,
                Comment = comment,
                IsPaidFor = paidFor.Value,
                IsCheckedIn = checkedIn.Value
            };

            databaseContext.Customers.Attach(customer);
            databaseContext.Rooms.Attach(room);
            databaseContext.Bookings.Add(booking);
            databaseContext.SaveChanges();

            _bookingList.Update();
        }
    }
}
