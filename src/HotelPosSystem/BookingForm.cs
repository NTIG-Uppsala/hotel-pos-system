﻿using HotelPosSystem.Entities;

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

        internal BookingForm(HotelDbContext databaseContext, BookingList bookingList) {
            _bookingList = bookingList;
            ContainerPanel = CreateBookingForm(databaseContext);
        }

        internal FlowLayoutPanel CreateBookingForm(HotelDbContext databaseContext) {
            FlowLayoutPanel bookingForm = new() {
                Name = "bookingForm",
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Dock = DockStyle.Fill
            };

            Label bookingFormHeading = new() {
                Text = "Create new booking",
                Font = new Font(bookingForm.Font.FontFamily, MainForm.HeadingFontSize),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, bottom: MainForm.MarginSize),
                UseCompatibleTextRendering = true
            };
            bookingForm.Controls.Add(bookingFormHeading);

            const int width = 400 + MainForm.MarginSize;

            Customer[] customers = databaseContext.Customers
                .OrderBy(customer => customer.FullName)
                .ToArray();
            (_customerDropdown, _) = ControlUtilities.AddComboBoxWithLabel(bookingForm, "customer", customers, width, "customerLabel", "Customer:");

            FlowLayoutPanel dateContainer = new() {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(0)
            };
            bookingForm.Controls.Add(dateContainer);
            (_startDatePicker, _) = ControlUtilities.AddDatePickerWithLabel(dateContainer, "startDate", DateTime.Now, "startDateLabel", "Start date:");
            (_endDatePicker, _) = ControlUtilities.AddDatePickerWithLabel(dateContainer, "endDate", DateTime.Now, "endDateLabel", "End date:");
            _startDatePicker.Margin = new Padding(_startDatePicker.Margin.Left, _startDatePicker.Margin.Top, right: MainForm.MarginSize, _startDatePicker.Margin.Bottom);
            _endDatePicker.Margin = new Padding(left: 0, _endDatePicker.Margin.Top, _endDatePicker.Margin.Right, _endDatePicker.Margin.Bottom);

            Room[] rooms = databaseContext.Rooms
                .Include(room => room.Type)
                .OrderBy(room => room.Name)
                .ToArray();
            (_roomDropdown, _) = ControlUtilities.AddComboBoxWithLabel(bookingForm, "room", rooms, width, "roomLabel", "Room:");

            (_commentTextBox, _) = ControlUtilities.AddTextBoxWithLabel(bookingForm, "comment", width, "commentLabel", "Comment:");

            TableLayoutPanel checkBoxContainer = new() {
                RowCount = 1,
                ColumnCount = 2,
                Width = width,
                Margin = new Padding(0)
            };
            checkBoxContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            checkBoxContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            checkBoxContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            bookingForm.Controls.Add(checkBoxContainer);
            (_paidForCheckBox, _) = ControlUtilities.AddCheckBoxWithLabel(checkBoxContainer, "paidFor", checkBoxState: false, checkBoxEnabled: true, "paidForLabel", "Paid for:");
            (_checkedInCheckBox, Label checkedInLabel) = ControlUtilities.AddCheckBoxWithLabel(checkBoxContainer, "checkedIn", checkBoxState: false, checkBoxEnabled: true, "checkedInLabel", "Checked in:");
            checkedInLabel.Margin = new Padding(left: MainForm.MarginSize / 2, checkedInLabel.Margin.Top, checkedInLabel.Margin.Right, checkedInLabel.Margin.Bottom);

            ControlUtilities.AddButton(bookingForm, "addBooking", "Add Booking", width, (sender, eventArgs) => CreateBooking());

            return bookingForm;
        }

        private void CreateBooking() {
            using HotelDbContext databaseContext = new();

            Customer? customer = _customerDropdown?.SelectedValue as Customer;
            DateOnly startDate = DateOnly.FromDateTime(_startDatePicker?.Value ?? new DateTime());
            DateOnly endDate = DateOnly.FromDateTime(_endDatePicker?.Value ?? new DateTime());
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

            _bookingList.Update(databaseContext);
        }
    }
}
