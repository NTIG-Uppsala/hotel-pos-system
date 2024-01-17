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

            TableLayoutPanel mainContainer = new() {
                RowCount = 1,
                ColumnCount = 2,
                Dock = DockStyle.Fill
            };

            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            FlowLayoutPanel bookingForm = CreateBookingForm(databaseContext);
            s_bookingList = CreateBookingList(databaseContext);

            mainContainer.Controls.Add(bookingForm);
            mainContainer.Controls.Add(s_bookingList);
            Controls.Add(mainContainer);
        }

        private static FlowLayoutPanel CreateBookingForm(HotelDbContext databaseContext) {
            FlowLayoutPanel bookingForm = new() {
                Name = "bookingForm",
                FlowDirection = FlowDirection.TopDown,
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

            const int width = 400 + MarginSize;

            Customer[] customers = databaseContext.Customers
                .OrderBy(customer => customer.FullName)
                .ToArray();
            (s_customerDropdown, _) = AddComboBoxAndLabel(bookingForm, "customer", customers, width, "customerLabel", "Customer:");

            FlowLayoutPanel dateContainer = new() {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(0)
            };
            bookingForm.Controls.Add(dateContainer);
            (s_startDatePicker, _) = AddDatePickerAndLabel(dateContainer, "startDate", DateTime.Now, "startDateLabel", "Start date:");
            (s_endDatePicker, _) = AddDatePickerAndLabel(dateContainer, "endDate", DateTime.Now, "endDateLabel", "End date:");
            s_startDatePicker.Margin = new Padding(s_startDatePicker.Margin.Left, s_startDatePicker.Margin.Top, right: MarginSize, s_startDatePicker.Margin.Bottom);
            s_endDatePicker.Margin = new Padding(left: 0, s_endDatePicker.Margin.Top, s_endDatePicker.Margin.Right, s_endDatePicker.Margin.Bottom);

            Room[] rooms = databaseContext.Rooms
                .Include(room => room.Type)
                .OrderBy(room => room.Name)
                .ToArray();
            (s_roomDropdown, _) = AddComboBoxAndLabel(bookingForm, "room", rooms, width, "roomLabel", "Room:");

            (s_commentTextBox, _) = AddTextBoxAndLabel(bookingForm, "comment", width, "commentLabel", "Comment:");

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
            (s_paidForCheckBox, _) = AddCheckBoxAndLabel(checkBoxContainer, "paidFor", checkBoxState: false, checkBoxEnabled: true, "paidForLabel", "Paid for:");
            (s_checkedInCheckBox, Label checkedInLabel) = AddCheckBoxAndLabel(checkBoxContainer, "checkedIn", checkBoxState: false, checkBoxEnabled: true, "checkedInLabel", "Checked in:");
            checkedInLabel.Margin = new Padding(left: MarginSize / 2, checkedInLabel.Margin.Top, checkedInLabel.Margin.Right, checkedInLabel.Margin.Bottom);

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
            AddLabel(flowLayoutPanel, "roomName" + booking.Id, "Room: " + booking.Room);

            // "o" date format corresponds to the ISO 8601 standard
            // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            string startDate = booking.StartDate.ToString("o", CultureInfo.InvariantCulture);
            string endDate = booking.EndDate.ToString("o", CultureInfo.InvariantCulture);
            AddLabel(flowLayoutPanel, "dates" + booking.Id,
                               $"Dates: {startDate} to {endDate}");

            AddCheckBoxAndLabel(flowLayoutPanel, "paidFor" + booking.Id, booking.IsPaidFor, checkBoxEnabled: false, "", "Has paid:");
            AddCheckBoxAndLabel(flowLayoutPanel, "checkedIn" + booking.Id, booking.IsCheckedIn, checkBoxEnabled: false, "", "Has checked in:");

            if (booking.Comment is not null) {
                AddLabel(flowLayoutPanel, "comment" + booking.Id, "Comment: " + booking.Comment);
            }

            return flowLayoutPanel;
        }

        private static (CheckBox, Label) AddCheckBoxAndLabel(Panel container, string checkBoxName, bool checkBoxState, bool checkBoxEnabled, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = Padding.Empty
            };

            Label label = AddLabel(layoutPanel, labelName, labelText);
            CheckBox checkBox = AddCheckBox(layoutPanel, checkBoxName, checkBoxState, checkBoxEnabled);

            container.Controls.Add(layoutPanel);
            return (checkBox, label);
        }

        private static Label AddLabel(Panel container, string name, string text) {
            Label label = new() {
                Name = name,
                Text = text,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            container.Controls.Add(label);
            return label;
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

        private static (ComboBox, Label) AddComboBoxAndLabel(Panel container, string comboBoxName, object[] comboBoxItems, int comboBoxWidth, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = Padding.Empty
            };

            Label label = AddLabel(layoutPanel, labelName, labelText);
            ComboBox comboBox = AddComboBox(layoutPanel, comboBoxName, comboBoxItems, comboBoxWidth);

            container.Controls.Add(layoutPanel);
            return (comboBox, label);
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

        private static (DateTimePicker, Label) AddDatePickerAndLabel(Panel container, string datePickerName, DateTime earliestDate, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = Padding.Empty
            };

            Label label = AddLabel(layoutPanel, labelName, labelText);
            DateTimePicker dateTimePicker = AddDatePicker(layoutPanel, datePickerName, earliestDate);

            container.Controls.Add(layoutPanel);
            return (dateTimePicker, label);
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

        private static (TextBox, Label) AddTextBoxAndLabel(Panel container, string textBoxName, int textBoxWidth, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = Padding.Empty
            };

            Label label = AddLabel(layoutPanel, labelName, labelText);
            TextBox textBox = AddTextBox(layoutPanel, textBoxName, textBoxWidth);

            container.Controls.Add(layoutPanel);
            return (textBox, label);
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
