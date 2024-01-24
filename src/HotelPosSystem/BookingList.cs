using System.Globalization;

using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    internal class BookingList {
        internal FlowLayoutPanel ContainerPanel;

        internal BookingList() {
            ContainerPanel = CreateListControls();
        }

        private FlowLayoutPanel CreateListControls() {
            using HotelDbContext databaseContext = new();

            FlowLayoutPanel listPanel = new() {
                Name = "bookingList",
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Dock = DockStyle.Fill,
                Margin = new Padding(MainForm.EdgeMarginSize)
            };

            Label heading = new() {
                Text = "Existing bookings",
                Font = new Font(listPanel.Font.FontFamily, MainForm.HeadingFontSize),
                Margin = new Padding(0, 0, 0, bottom: MainForm.MarginSize),
                AutoSize = true,
                UseCompatibleTextRendering = true
            };
            listPanel.Controls.Add(heading);

            Booking[] bookings = databaseContext.Bookings
                .Include(booking => booking.Customer)
                .Include(booking => booking.Room)
                .Include(booking => booking.Room.Type)
                .OrderBy(booking => booking.StartDate)
                .ToArray();

            foreach (Booking booking in bookings) {
                listPanel.Controls.Add(CreateListItem(booking));
            }

            return listPanel;
        }

        private FlowLayoutPanel CreateListItem(Booking booking) {
            FlowLayoutPanel flowLayoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, bottom: MainForm.MarginSize)
            };

            ControlUtilities.AddReadOnlyTextBox(flowLayoutPanel, "customerName" + booking.Id, "Name: " + booking.Customer.FullName);
            ControlUtilities.AddReadOnlyTextBox(flowLayoutPanel, "emailAddress" + booking.Id, "Email address: " + booking.Customer.EmailAddress);
            ControlUtilities.AddReadOnlyTextBox(flowLayoutPanel, "phoneNumber" + booking.Id, "Phone number: " + booking.Customer.PhoneNumber);
            ControlUtilities.AddReadOnlyTextBox(flowLayoutPanel, "roomName" + booking.Id, "Room: " + booking.Room);

            // "o" date format corresponds to the ISO 8601 standard
            // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            string startDate = booking.StartDate.ToString("o", CultureInfo.InvariantCulture);
            string endDate = booking.EndDate.ToString("o", CultureInfo.InvariantCulture);
            ControlUtilities.AddReadOnlyTextBox(flowLayoutPanel, "dates" + booking.Id,
                               $"Dates: {startDate} to {endDate}");

            ControlUtilities.AddCheckBoxWithLabel(flowLayoutPanel, "paidFor" + booking.Id, booking.IsPaidFor, checkBoxEnabled: false, "", "Has paid:");
            ControlUtilities.AddCheckBoxWithLabel(flowLayoutPanel, "checkedIn" + booking.Id, booking.IsCheckedIn, checkBoxEnabled: false, "", "Has checked in:");

            if (booking.Comment is not null) {
                ControlUtilities.AddReadOnlyTextBox(flowLayoutPanel, "comment" + booking.Id, "Comment: " + booking.Comment);
            }

            const int removeButtonWidth = 150;
            ControlUtilities.AddButton(flowLayoutPanel, "removeButton" + booking.Id, "Remove", removeButtonWidth, (object? sender, EventArgs eventArgs) => ShowRemoveBookingModal(booking.Id));

            return flowLayoutPanel;
        }

        private void ShowRemoveBookingModal(int bookingId) {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this booking?", "Remove booking", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                RemoveBooking(bookingId);
            }
        }

        private void RemoveBooking(int id) {
            using HotelDbContext databaseContext = new();
            databaseContext.Bookings.Remove(databaseContext.Bookings.First(booking => booking.Id == id));
            databaseContext.SaveChanges();
            Update();
        }

        internal void Update() {
            Control? bookingListParent = ContainerPanel?.Parent;
            bookingListParent?.Controls.Remove(ContainerPanel);
            ContainerPanel = CreateListControls();
            bookingListParent?.Controls.Add(ContainerPanel);
        }
    }
}
