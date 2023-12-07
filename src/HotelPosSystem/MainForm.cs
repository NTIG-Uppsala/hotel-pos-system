namespace HotelPosSystem {
    public partial class MainForm : Form {
        private readonly uint[] _occupiedRooms;
        private readonly Label[] _roomTypeCounterTexts;

        public MainForm() {
            InitializeComponent();

            using HotelDbContext databaseContext = new();

            CreateRoomTypesIfEmpty(databaseContext, "Single Room", "Double Room");

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
            RoomType[] roomTypes = databaseContext.RoomTypes.ToArray();
            for (int i = 0; i < roomTypes.Length; i++) {
                FlowLayoutPanel row = CreateCounterRow(i);
                verticalLayoutPanel.Controls.Add(row);
            }

            Controls.Add(verticalLayoutPanel);
        }

        private static void CreateRoomTypesIfEmpty(HotelDbContext databaseContext, params string[] names) {
            if (!databaseContext.RoomTypes.Any()) {
                foreach (string name in names) {
                    RoomType roomType = new() {
                        Name = name
                    };
                    databaseContext.Add(roomType);
                }
                databaseContext.SaveChanges();
            }
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
            RoomType roomType = GetRoomType(index);
            string newText = $"{roomType.Name}: {_occupiedRooms[index]}";
            _roomTypeCounterTexts[index].Text = newText;
        }

        private FlowLayoutPanel CreateCounterRow(int index) {
            RoomType roomType = GetRoomType(index);
            FlowLayoutPanel result = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Name = $"{roomType.Name} Row"
            };

            Label counterText = new() {
                Name = "roomTypeCounterText",
                AutoSize = true
            };
            _roomTypeCounterTexts[index] = counterText;
            UpdateRoomTypeCounterText(index);
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

        private static RoomType GetRoomType(int index) {
            using HotelDbContext databaseContext = new();
            return databaseContext.RoomTypes
                .OrderBy(roomType => roomType.Id)
                .ToArray()[index];
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
