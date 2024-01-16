using System.Globalization;

using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    public partial class MainForm : Form {
        private const int HeadingFontSize = 18;
        private const int MarginSize = 30;

        private static ComboBox? s_customerDropdown;
        private static DateTimePicker? s_startDatePicker;
        private static DateTimePicker? s_endDatePicker;
        private static ComboBox? s_roomDropdown;
        private static TextBox? s_commentTextBox;
        private static CheckBox? s_paidForCheckBox;
        private static CheckBox? s_checkedInCheckBox;

        private static FlowLayoutPanel? s_bookingList;

        public MainForm() {
            InitializeComponent();

            using HotelDbContext databaseContext = new();
            DatabaseUtilities.SetUpDatabase(databaseContext);

            TableLayoutPanel container = new() {
                RowCount = 1,
                ColumnCount = 2,
                Dock = DockStyle.Fill
            };

            container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            container.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            FlowLayoutPanel bookingForm = CreateBookingForm(databaseContext);
            s_bookingList = CreateBookingList(databaseContext);

            container.Controls.Add(bookingForm);
            container.Controls.Add(s_bookingList);
            Controls.Add(container);
        }

        private static FlowLayoutPanel CreateBookingForm(HotelDbContext databaseContext) {
            FlowLayoutPanel bookingForm = new() {
                Name = "bookingForm",
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                AutoScroll = true,
                Dock = DockStyle.Fill
            };

            Label bookingFormHeading = new() {
                Text = "Create new booking",
                Font = new Font(DefaultFont.FontFamily, HeadingFontSize),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, bottom: MarginSize),
                UseCompatibleTextRendering = true
            };
            bookingForm.Controls.Add(bookingFormHeading);

            const int width = 300;

            Customer[] customers = databaseContext.Customers
                .OrderBy(customer => customer.FullName)
                .ToArray();
            s_customerDropdown = AddComboBox(bookingForm, "customer", customers, width);

            s_startDatePicker = AddDatePicker(bookingForm, "startDate", DateTime.Now);
            s_endDatePicker = AddDatePicker(bookingForm, "endDate", DateTime.Now);

            Room[] rooms = databaseContext.Rooms
                .OrderBy(room => room.Name)
                .ToArray();
            s_roomDropdown = AddComboBox(bookingForm, "room", rooms, width);

            s_commentTextBox = AddTextBox(bookingForm, "comment", width);

            s_paidForCheckBox = AddCheckBox(bookingForm, "paidFor", false, true);
            s_checkedInCheckBox = AddCheckBox(bookingForm, "checkIn", false, true);

            AddButton(bookingForm, "addBooking", "Add Booking", width, (sender, eventArgs) => CreateBooking());

            return bookingForm;
        }

        private static FlowLayoutPanel CreateBookingList(HotelDbContext databaseContext) {
            FlowLayoutPanel bookingList = new() {
                Name = "bookingList",
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Dock = DockStyle.Fill
            };

            Label bookingListHeading = new() {
                Text = "Existing bookings",
                Font = new Font(DefaultFont.FontFamily, HeadingFontSize),
                AutoSize = true,
                UseCompatibleTextRendering = true
            };
            bookingList.Controls.Add(bookingListHeading);

            Booking[] bookings = databaseContext.Bookings
                .Include(booking => booking.Customer)
                .Include(booking => booking.Room)
                .OrderBy(booking => booking.StartDate)
                .ToArray();

            foreach (Booking booking in bookings) {
                bookingList.Controls.Add(CreateListItem(booking));
            }

            return bookingList;
        }

        private static void UpdateBookingList(HotelDbContext databaseContext) {
            Control? bookingListParent = s_bookingList?.Parent;
            bookingListParent?.Controls.Remove(s_bookingList);
            s_bookingList = CreateBookingList(databaseContext);
            bookingListParent?.Controls.Add(s_bookingList);
        }

        private static FlowLayoutPanel CreateListItem(Booking booking) {
            FlowLayoutPanel flowLayoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(0, top: MarginSize, 0, 0)
            };

            AddLabel(flowLayoutPanel, "customerName" + booking.Id, "Name: " + booking.Customer.FullName);
            AddLabel(flowLayoutPanel, "emailAddress" + booking.Id, "Email address: " + booking.Customer.EmailAdress);
            AddLabel(flowLayoutPanel, "phoneNumber" + booking.Id, "Phone number: " + booking.Customer.PhoneNumber);
            AddLabel(flowLayoutPanel, "roomName" + booking.Id, "Room: " + booking.Room.Name);

            // "o" date format corresponds to the ISO 8601 standard
            // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            string startDate = booking.StartDate.ToString("o", CultureInfo.InvariantCulture);
            string endDate = booking.EndDate.ToString("o", CultureInfo.InvariantCulture);
            AddLabel(flowLayoutPanel, "dates" + booking.Id,
                               $"Dates: {startDate} to {endDate}");

            AddCheckBoxAndLabel(flowLayoutPanel, "paidFor" + booking.Id, booking.IsPaidFor, "", "Has paid:");
            AddCheckBoxAndLabel(flowLayoutPanel, "checkedIn" + booking.Id, booking.IsCheckedIn, "", "Has checked in:");

            if (booking.Comment is not null) {
                AddLabel(flowLayoutPanel, "comment" + booking.Id, "Comment: " + booking.Comment);
            }

            return flowLayoutPanel;
        }

        private static void AddCheckBoxAndLabel(Panel container, string checkBoxName, bool checkBoxState, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = Padding.Empty
            };

            AddLabel(layoutPanel, labelName, labelText);
            AddCheckBox(layoutPanel, checkBoxName, checkBoxState, isEnabled: false);

            container.Controls.Add(layoutPanel);
        }

        private static void AddLabel(Panel container, string name, string text) {
            Label label = new() {
                Name = name,
                Text = text,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            container.Controls.Add(label);
        }

        private static CheckBox AddCheckBox(Panel container, string name, bool isChecked, bool isEnabled) {
            CheckBox checkBox = new() {
                Name = name,
                Checked = isChecked,
                Enabled = isEnabled,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            container.Controls.Add(checkBox);
            return checkBox;
        }

        private static ComboBox AddComboBox(Panel container, string name, object[] items, int width) {
            ComboBox comboBox = new() {
                Name = name,
                DataSource = items,
                Width = width,
            };
            container.Controls.Add(comboBox);
            return comboBox;
        }

        private static DateTimePicker AddDatePicker(Panel container, string name, DateTime earliestDate) {
            DateTimePicker datePicker = new() {
                Name = name,
                MinDate = earliestDate,
                Format = DateTimePickerFormat.Short
            };
            container.Controls.Add(datePicker);
            return datePicker;
        }

        private static TextBox AddTextBox(Panel container, string name, int width) {
            TextBox textBox = new() {
                Name = name,
                Width = width
            };
            container.Controls.Add(textBox);
            return textBox;
        }

        private static void AddButton(Panel container, string name, string text, int width, EventHandler onClick) {
            Button button = new() {
                Name = name,
                Text = text,
                Width = width,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            button.Click += onClick;
            container.Controls.Add(button);
        }

        private static void CreateBooking() {
            using HotelDbContext databaseContext = new();

            Customer? customer = s_customerDropdown?.SelectedValue as Customer;
            DateOnly startDate = DateOnly.FromDateTime(s_startDatePicker?.Value ?? new DateTime());
            DateOnly endDate = DateOnly.FromDateTime(s_endDatePicker?.Value ?? new DateTime());
            Room? room = s_roomDropdown?.SelectedValue as Room;
            string? comment = s_commentTextBox?.Text;
            bool? paidFor = s_paidForCheckBox?.Checked;
            bool? checkedIn = s_checkedInCheckBox?.Checked;

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

            UpdateBookingList(databaseContext);
        }
    }
}
