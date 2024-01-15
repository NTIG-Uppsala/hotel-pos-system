using System.Globalization;

using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    public partial class MainForm : Form {
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

            FlowLayoutPanel bookingList = new() {
                Name = "bookingList",
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Dock = DockStyle.Fill
            };

            FlowLayoutPanel bookingForm = CreateBookingForm(databaseContext);

            Label bookingListHeading = new() {
                Text = "Existing bookings",
                Font = new Font(Font.FontFamily, 18),
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

            container.Controls.Add(bookingForm);
            container.Controls.Add(bookingList);
            Controls.Add(container);
        }

        private static FlowLayoutPanel CreateBookingForm(HotelDbContext databaseContext) {
            FlowLayoutPanel flowLayoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            const int width = 300;

            Customer[] customers = databaseContext.Customers
                .OrderBy(customer => customer.FullName)
                .ToArray();
            AddComboBox(flowLayoutPanel, "customer", customers, width);

            AddDatePicker(flowLayoutPanel, "startDate", DateTime.Now);
            AddDatePicker(flowLayoutPanel, "endDate", DateTime.Now);

            Room[] rooms = databaseContext.Rooms
                .OrderBy(room => room.Name)
                .ToArray();
            AddComboBox(flowLayoutPanel, "room", rooms, width);

            AddTextBox(flowLayoutPanel, "comment", width);

            AddCheckBox(flowLayoutPanel, "paidFor", false, true);
            AddCheckBox(flowLayoutPanel, "checkIn", false, true);

            AddButton(flowLayoutPanel, "addBooking", "Add Booking", width);

            return flowLayoutPanel;
        }

        private static FlowLayoutPanel CreateListItem(Booking booking) {
            FlowLayoutPanel flowLayoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(0, top: 30, 0, 0)
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

        private static void AddCheckBox(Panel container, string name, bool isChecked, bool isEnabled) {
            CheckBox checkBox = new() {
                Name = name,
                Checked = isChecked,
                Enabled = isEnabled,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            container.Controls.Add(checkBox);
        }

        private static void AddComboBox(Panel container, string name, object[] items, int width) {
            ComboBox comboBox = new() {
                Name = name,
                DataSource = items,
                Width = width,
            };
            container.Controls.Add(comboBox);
        }

        private static void AddDatePicker(Panel container, string name, DateTime earliestDate) {
            DateTimePicker datePicker = new() {
                Name = name,
                MinDate = earliestDate,
                Format = DateTimePickerFormat.Short
            };
            container.Controls.Add(datePicker);
        }

        private static void AddTextBox(Panel container, string name, int width) {
            TextBox textBox = new() {
                Name = name,
                Width = width
            };
            container.Controls.Add(textBox);
        }

        private static void AddButton(Panel container, string name, string text, int width) {
            Button button = new() {
                Name = name,
                Text = text,
                Width = width,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            container.Controls.Add(button);
        }
    }
}
