using HotelPosSystem.Entities;

namespace HotelPosSystem {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();

            using HotelDbContext databaseContext = new();
            DatabaseUtilities.SetUpDatabase(databaseContext);
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
