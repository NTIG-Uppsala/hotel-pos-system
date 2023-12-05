namespace HotelPosSystem {
    public partial class MainForm : Form {
        private uint _occupiedRooms = 0;
        private readonly Label _occupiedRoomsText;

        public MainForm() {
            InitializeComponent();

            using HotelDbContext databaseContext = new();

            RoomType? roomType = CreateRoomTypeIfEmpty(databaseContext);

            roomType ??= databaseContext.RoomTypes.First();

            FlowLayoutPanel verticalLayoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            Label roomTypeHeading = new() {
                Text = roomType.Name,
                Font = new Font(Font.FontFamily, 14)
            };
            verticalLayoutPanel.Controls.Add(roomTypeHeading);

            FlowLayoutPanel horizontalLayoutPanel = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };
            verticalLayoutPanel.Controls.Add(horizontalLayoutPanel);

            _occupiedRoomsText = new Label() {
                Name = "occupiedRoomsText",
                AutoSize = true
            };
            horizontalLayoutPanel.Controls.Add(_occupiedRoomsText);

            Button incrementButton =
                CreateButton("incrementButton", "+", OnIncrementButtonClicked);
            horizontalLayoutPanel.Controls.Add(incrementButton);

            Button decrementButton =
                CreateButton("decrementButton", "-", OnDecrementButtonClicked);
            horizontalLayoutPanel.Controls.Add(decrementButton);

            Button resetButton =
                CreateButton("resetButton", "Reset", OnResetButtonClicked);
            horizontalLayoutPanel.Controls.Add(resetButton);

            UpdateOccupiedRoomsText();
            Controls.Add(verticalLayoutPanel);
        }

        private RoomType? CreateRoomTypeIfEmpty(HotelDbContext databaseContext) {
            if (databaseContext.RoomTypes.Count() == 0) {
                RoomType roomType = new() {
                    Name = "Room"
                };
                databaseContext.Add(roomType);
                databaseContext.SaveChanges();
                return roomType;
            }
            return null;
        }

        private void OnIncrementButtonClicked(object? sender, EventArgs e) {
            _occupiedRooms++;
            UpdateOccupiedRoomsText();
        }

        private void OnDecrementButtonClicked(object? sender, EventArgs e) {
            if (_occupiedRooms > 0) {
                _occupiedRooms--;
                UpdateOccupiedRoomsText();
            }
        }

        private void OnResetButtonClicked(object? sender, EventArgs e) {
            _occupiedRooms = 0;
            UpdateOccupiedRoomsText();
        }

        private void UpdateOccupiedRoomsText() {
            string newText = $"Occupied rooms: {_occupiedRooms}";
            _occupiedRoomsText.Text = newText;
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
