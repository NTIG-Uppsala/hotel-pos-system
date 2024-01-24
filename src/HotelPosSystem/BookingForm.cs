using System.ComponentModel;

using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    internal class BookingForm {
        private readonly BookingList _bookingList;

        private ComboBox? _customerDropdown;
        private Label? _customerErrorLabel;
        private DateTimePicker? _startDatePicker;
        private DateTimePicker? _endDatePicker;
        private Label? _dateErrorLabel;
        private ComboBox? _roomDropdown;
        private Label? _roomErrorLabel;
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
                Dock = DockStyle.Fill,
                Margin = new Padding(MainForm.EdgeMarginSize)
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
            (_customerDropdown, _) = ControlUtilities.AddComboBoxWithLabel(formPanel, "customer", customers, "Select a customer", width, "customerLabel", "Customer:");
            _customerDropdown.Validating += (object? sender, CancelEventArgs eventArgs) => ValidateCustomer();
            _customerErrorLabel = ControlUtilities.AddLabel(formPanel, "customerError", string.Empty);
            _customerErrorLabel.ForeColor = Color.Red;

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
            _startDatePicker.Validating += (object? sender, CancelEventArgs eventArgs) => ValidateDates();
            _endDatePicker.Validating += (object? sender, CancelEventArgs eventArgs) => ValidateDates();
            _dateErrorLabel = ControlUtilities.AddLabel(formPanel, "dateError", string.Empty);
            _dateErrorLabel.ForeColor = Color.Red;

            Room[] rooms = databaseContext.Rooms
                .Include(room => room.Type)
                .OrderBy(room => room.Name)
                .ToArray();
            (_roomDropdown, _) = ControlUtilities.AddComboBoxWithLabel(formPanel, "room", rooms, "Select a room", width, "roomLabel", "Room:");
            _roomDropdown.Validating += (object? sender, CancelEventArgs eventArgs) => ValidateRoom();
            _roomErrorLabel = ControlUtilities.AddLabel(formPanel, "roomError", string.Empty);
            _roomErrorLabel.ForeColor = Color.Red;

            const int checkboxContainerMaxHeight = 22;
            TableLayoutPanel checkBoxContainer = new() {
                RowCount = 1,
                ColumnCount = 2,
                MinimumSize = new Size(width, 0),
                MaximumSize = new Size(width, checkboxContainerMaxHeight),
                AutoSize = true,
                Margin = new Padding(0)
            };
            ControlUtilities.CreateRowsAndColumns(checkBoxContainer);
            formPanel.Controls.Add(checkBoxContainer);
            (_paidForCheckBox, _) = ControlUtilities.AddCheckBoxWithLabel(checkBoxContainer, "paidFor", checkBoxState: false, checkBoxEnabled: true, "paidForLabel", "Paid for:");
            (_checkedInCheckBox, Label checkedInLabel) = ControlUtilities.AddCheckBoxWithLabel(checkBoxContainer, "checkedIn", checkBoxState: false, checkBoxEnabled: true, "checkedInLabel", "Checked in:");
            checkedInLabel.Margin = new Padding(left: MainForm.MarginSize / 2, checkedInLabel.Margin.Top, checkedInLabel.Margin.Right, checkedInLabel.Margin.Bottom);

            (_commentTextBox, _) = ControlUtilities.AddTextBoxWithLabel(formPanel, "comment", width, "commentLabel", "Comment:");

            ControlUtilities.AddButton(formPanel, "addBooking", "Add Booking", width, (sender, eventArgs) => CreateBooking());

            // Ensure default values in form
            EmptyForm();

            return formPanel;
        }

        private void CreateBooking() {
            using HotelDbContext databaseContext = new();

            Customer? customer = _customerDropdown?.SelectedValue as Customer;
            if (_startDatePicker is null || _endDatePicker is null
                || _customerErrorLabel is null || _dateErrorLabel is null || _roomErrorLabel is null) {
                throw new NullReferenceException();
            }
            DateOnly startDate = DateOnly.FromDateTime(_startDatePicker.Value);
            DateOnly endDate = DateOnly.FromDateTime(_endDatePicker.Value);
            Room? room = _roomDropdown?.SelectedValue as Room;
            string? comment = string.IsNullOrWhiteSpace(_commentTextBox?.Text) ? null : _commentTextBox.Text;
            bool? paidFor = _paidForCheckBox?.Checked;
            bool? checkedIn = _checkedInCheckBox?.Checked;

            if (paidFor is null || checkedIn is null) {
                throw new NullReferenceException();
            }

            if (!ValidateFormData()) {
                return;
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
            EmptyForm();
        }

        private void EmptyForm() {
            if (_commentTextBox is null || _endDatePicker is null || _endDatePicker is null
                || _startDatePicker is null || _checkedInCheckBox is null || _paidForCheckBox is null
                || _roomDropdown is null || _customerDropdown is null) {
                throw new NullReferenceException();
            };

            if (_customerDropdown.Items.Count > 0) {
                _customerDropdown.SelectedIndex = 0;
            }

            _startDatePicker.Value = _startDatePicker.MinDate;
            _endDatePicker.Value = _endDatePicker.MinDate.AddDays(1);

            if (_roomDropdown.Items.Count > 0) {
                _roomDropdown.SelectedIndex = 0;
            }

            _commentTextBox.Text = string.Empty;

            _paidForCheckBox.Checked = false;
            _checkedInCheckBox.Checked = false;
        }

        private bool ValidateFormData() {
            return ValidateCustomer() & ValidateDates() & ValidateRoom();
        }

        private bool ValidateCustomer() {
            Customer? customer = _customerDropdown?.SelectedValue as Customer;
            if (customer is null) {
                _customerErrorLabel.Text = "Please select a customer";
                return false;
            }

            _customerErrorLabel.Text = string.Empty;
            return true;
        }

        private bool ValidateDates() {
            DateOnly startDate = DateOnly.FromDateTime(_startDatePicker.Value);
            DateOnly endDate = DateOnly.FromDateTime(_endDatePicker.Value);

            if (endDate < startDate) {
                _dateErrorLabel.Text = "End date is before start date";
                return false;
            }

            _dateErrorLabel.Text = string.Empty;
            return true;
        }

        private bool ValidateRoom() {
            Room? room = _roomDropdown?.SelectedValue as Room;
            if (room is null) {
                _roomErrorLabel.Text = "Please select a room";
                return false;
            }

            _roomErrorLabel.Text = string.Empty;
            return true;
        }
    }
}
