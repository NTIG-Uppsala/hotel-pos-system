using System.Globalization;

using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    internal class BookingList {
        internal FlowLayoutPanel ContainerPanel;

        internal BookingList() {
            ContainerPanel = CreateListControls();
        }

        private static FlowLayoutPanel CreateListControls() {
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

        private static FlowLayoutPanel CreateListItem(Booking booking) {
            FlowLayoutPanel flowLayoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, bottom: MainForm.MarginSize)
            };

            ControlUtilities.AddLabel(flowLayoutPanel, "customerName" + booking.Id, "Name: " + booking.Customer.FullName);
            ControlUtilities.AddLabel(flowLayoutPanel, "emailAddress" + booking.Id, "Email address: " + booking.Customer.EmailAdress);
            ControlUtilities.AddLabel(flowLayoutPanel, "phoneNumber" + booking.Id, "Phone number: " + booking.Customer.PhoneNumber);
            ControlUtilities.AddLabel(flowLayoutPanel, "roomName" + booking.Id, "Room: " + booking.Room);

            // "o" date format corresponds to the ISO 8601 standard
            // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            string startDate = booking.StartDate.ToString("o", CultureInfo.InvariantCulture);
            string endDate = booking.EndDate.ToString("o", CultureInfo.InvariantCulture);
            ControlUtilities.AddLabel(flowLayoutPanel, "dates" + booking.Id,
                               $"Dates: {startDate} to {endDate}");

            ControlUtilities.AddCheckBoxWithLabel(flowLayoutPanel, "paidFor" + booking.Id, booking.IsPaidFor, checkBoxEnabled: false, "", "Has paid:");
            ControlUtilities.AddCheckBoxWithLabel(flowLayoutPanel, "checkedIn" + booking.Id, booking.IsCheckedIn, checkBoxEnabled: false, "", "Has checked in:");

            if (booking.Comment is not null) {
                ControlUtilities.AddLabel(flowLayoutPanel, "comment" + booking.Id, "Comment: " + booking.Comment);
            }

            return flowLayoutPanel;
        }

        internal void Update() {
            Control? bookingListParent = ContainerPanel?.Parent;
            bookingListParent?.Controls.Remove(ContainerPanel);
            ContainerPanel = CreateListControls();
            bookingListParent?.Controls.Add(ContainerPanel);
        }
    }
}
