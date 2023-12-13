using System.Globalization;

using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();

            using HotelDbContext databaseContext = new();
            DatabaseUtilities.SetUpDatabase(databaseContext);

            FlowLayoutPanel bookingList = new() {
                Name = "bookingList",
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Dock = DockStyle.Fill
            };

            Booking[] test = databaseContext.Bookings.ToArray();

            Booking[] bookings = databaseContext.Bookings
                .Include(booking => booking.Customer)
                .Include(booking => booking.Room)
                .OrderBy(booking => booking.StartDate)
                .ToArray();

            foreach (Booking booking in bookings) {
                bookingList.Controls.Add(CreateListItem(booking));
            }
            Controls.Add(bookingList);
        }

        private static FlowLayoutPanel CreateListItem(Booking booking) {
            FlowLayoutPanel flowLayoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, bottom: 40)
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

            AddCheckBoxAndLabel(flowLayoutPanel, "paidFor" + booking.Id, booking.IsPayedFor, "", "Has paid:");
            AddCheckBoxAndLabel(flowLayoutPanel, "checkedIn" + booking.Id, booking.IsCheckedIn, "", "Has checked in:");

            if (booking.Comment is not null) {
                AddLabel(flowLayoutPanel, "comment" + booking.Id, "Comment: " + booking.Comment);
            }

            return flowLayoutPanel;
        }

        private static void AddCheckBoxAndLabel(Panel container, string checkBoxName, bool checkBoxState, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
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
                UseCompatibleTextRendering = true
            };
            container.Controls.Add(checkBox);
        }

        private static Button CreateButton(string name, string text, EventHandler? onClicked) {
            Button button = new() {
                Name = name,
                Text = text,
                UseCompatibleTextRendering = true
            };
            button.Click += onClicked;
            return button;
        }
    }
}
