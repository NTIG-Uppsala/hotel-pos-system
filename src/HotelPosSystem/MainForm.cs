namespace HotelPosSystem {
    public partial class MainForm : Form {
        private readonly uint[] _occupiedRooms;
        private readonly Label[] _roomTypeCounterTexts;

        public MainForm() {
            InitializeComponent();

            using HotelDbContext databaseContext = new();

            _ = CreateRoomTypeIfEmpty(databaseContext, "Single Room");

            _occupiedRooms = new uint[databaseContext.RoomTypes.Count()];
            _roomTypeCounterTexts = new Label[_occupiedRooms.Length];

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

            FlowLayoutPanel row = CreateCounterRow(0);
            verticalLayoutPanel.Controls.Add(row);

            Controls.Add(verticalLayoutPanel);
        }

        private RoomType? CreateRoomTypeIfEmpty(HotelDbContext databaseContext, string name) {
            if (!databaseContext.RoomTypes.Any()) {
                RoomType roomType = new() {
                    Name = name
                };
                databaseContext.Add(roomType);
                databaseContext.SaveChanges();
                return roomType;
            }
            return null;
        }

        private void OnIncrementButtonClicked(int index) {
            _occupiedRooms[index]++;
            UpdateRoomTypeCounterText(index);
        }

        private void OnDecrementButtonClicked(int index) {
            if (_occupiedRooms[index] > 0) {
                _occupiedRooms[index]--;
                UpdateRoomTypeCounterText(index);
            }
        }

        private void OnResetButtonClicked(int index) {
            _occupiedRooms[index] = 0;
            UpdateRoomTypeCounterText(index);
        }

        private void UpdateRoomTypeCounterText(int index) {
            using HotelDbContext databaseContext = new();
            RoomType roomType = databaseContext.RoomTypes
                .OrderBy(roomType => roomType.Id)
                .ToArray()[index];
            string newText = $"{roomType.Name}: {_occupiedRooms[index]}";
            _roomTypeCounterTexts[index].Text = newText;
        }

        private FlowLayoutPanel CreateCounterRow(int index) {
            FlowLayoutPanel result = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };

            Label counterText = new() {
                Name = "roomTypeCounterText",
                AutoSize = true
            };
            _roomTypeCounterTexts[index] = counterText;
            UpdateRoomTypeCounterText(0);
            result.Controls.Add(counterText);

            Button decrementButton =
              CreateButton("decrementButton", "-",
              (object? sender, EventArgs eventArgs) => OnDecrementButtonClicked(index));
            result.Controls.Add(decrementButton);

            Button incrementButton =
                CreateButton("incrementButton", "+",
                (object? sender, EventArgs eventArgs) => OnIncrementButtonClicked(index));
            result.Controls.Add(incrementButton);

            Button resetButton =
                CreateButton("resetButton", "Reset",
                (object? sender, EventArgs eventArgs) => OnResetButtonClicked(index));
            result.Controls.Add(resetButton);

            return result;
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
