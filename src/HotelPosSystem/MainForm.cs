namespace HotelPosSystem {
    public partial class MainForm : Form {
        internal const int HeadingFontSize = 18;
        internal const int MarginSize = 30;

        public MainForm() {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            DatabaseUtilities.SetUpDatabase();
            if (DatabaseUtilities.AreAllTablesEmpty()) {
                DatabaseUtilities.SeedDatabase();
            }

            TableLayoutPanel mainContainer = new() {
                RowCount = 1,
                ColumnCount = 2,
                Dock = DockStyle.Fill
            };
            ControlUtilities.CreateRowsAndColumns(mainContainer);

            BookingList bookingList = new();
            BookingForm bookingForm = new(bookingList);

            mainContainer.Controls.Add(bookingForm.ContainerPanel);
            mainContainer.Controls.Add(bookingList.ContainerPanel);
            Controls.Add(mainContainer);

            ActiveControl = mainContainer;
        }
    }
}
