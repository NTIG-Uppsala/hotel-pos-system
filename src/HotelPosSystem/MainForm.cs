namespace HotelPosSystem {
    public partial class MainForm : Form {
        internal const int HeadingFontSize = 18;
        internal const int MarginSize = 30;

        public MainForm() {
            InitializeComponent();

            DatabaseUtilities.SetUpDatabase();

            TableLayoutPanel mainContainer = new() {
                RowCount = 1,
                ColumnCount = 2,
                Dock = DockStyle.Fill
            };

            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            BookingList bookingList = new();
            BookingForm bookingForm = new(bookingList);

            mainContainer.Controls.Add(bookingForm.ContainerPanel);
            mainContainer.Controls.Add(bookingList.ContainerPanel);
            Controls.Add(mainContainer);
        }
    }
}
