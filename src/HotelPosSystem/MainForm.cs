namespace HotelPosSystem {
    public partial class MainForm : Form {
        private uint _occupiedRooms = 0;
        private readonly Label _roomTypeCounterText;

        public MainForm() {
            InitializeComponent();

            using HotelDbContext databaseContext = new();

            RoomType? roomType = CreateRoomTypeIfEmpty(databaseContext, "Single Room");

            roomType ??= databaseContext.RoomTypes.First();

            FlowLayoutPanel verticalLayoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            Label occupiedRoomsHeading = new() {
                Text = "Occupied Rooms",
                Font = new Font(Font.FontFamily, 14),
                AutoSize = true
            };
            verticalLayoutPanel.Controls.Add(occupiedRoomsHeading);

            FlowLayoutPanel horizontalLayoutPanel = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };
            verticalLayoutPanel.Controls.Add(horizontalLayoutPanel);

            _roomTypeCounterText = new Label() {
                Name = "roomTypeCounterText",
                AutoSize = true
            };
            horizontalLayoutPanel.Controls.Add(_roomTypeCounterText);

            Button incrementButton =
                CreateButton("incrementButton", "+", OnIncrementButtonClicked);
            horizontalLayoutPanel.Controls.Add(incrementButton);

            Button decrementButton =
                CreateButton("decrementButton", "-", OnDecrementButtonClicked);
            horizontalLayoutPanel.Controls.Add(decrementButton);

            Button resetButton =
                CreateButton("resetButton", "Reset", OnResetButtonClicked);
            horizontalLayoutPanel.Controls.Add(resetButton);

            UpdateRoomTypeCounterText();
            Controls.Add(verticalLayoutPanel);
        }

        private RoomType? CreateRoomTypeIfEmpty(HotelDbContext databaseContext, string name) {
            if (databaseContext.RoomTypes.Count() == 0) {
                RoomType roomType = new() {
                    Name = name
                };
                databaseContext.Add(roomType);
                databaseContext.SaveChanges();
                return roomType;
            }
            return null;
        }

        private void OnIncrementButtonClicked(object? sender, EventArgs e) {
            _occupiedRooms++;
            UpdateRoomTypeCounterText();
        }

        private void OnDecrementButtonClicked(object? sender, EventArgs e) {
            if (_occupiedRooms > 0) {
                _occupiedRooms--;
                UpdateRoomTypeCounterText();
            }
        }

        private void OnResetButtonClicked(object? sender, EventArgs e) {
            _occupiedRooms = 0;
            UpdateRoomTypeCounterText();
        }

        private void UpdateRoomTypeCounterText() {
            using HotelDbContext databaseContext = new();
            string newText = $"{databaseContext.RoomTypes.First().Name}: {_occupiedRooms}";
            _roomTypeCounterText.Text = newText;
        }

        private static Button CreateButton(string name, string text, EventHandler? onClicked) {
            Button button = new() {
                Name = name,
                Text = text
            };
            button.UseCompatibleTextRendering = true;
            button.Click += onClicked;
            return button;
        }
    }
}
