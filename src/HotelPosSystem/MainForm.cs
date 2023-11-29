
namespace HotelPosSystem {
    public partial class MainForm : Form {
        private uint _occupiedRooms = 0;
        private readonly Label _occupiedRoomsText;

        public MainForm() {
            InitializeComponent();
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };

            _occupiedRoomsText = new Label() {
                Name = "occupiedRoomsText",
                AutoSize = true
            };
            layoutPanel.Controls.Add(_occupiedRoomsText);

            Button incrementButton =
                CreateButton("incrementButton", "+", OnIncrementButtonClicked);
            layoutPanel.Controls.Add(incrementButton);

            Button decrementButton =
                CreateButton("decrementButton", "-", OnDecrementButtonClicked);
            layoutPanel.Controls.Add(decrementButton);

            Button resetButton =
                CreateButton("resetButton", "Reset", OnResetButtonClicked);
            layoutPanel.Controls.Add(resetButton);

            UpdateOccupiedRoomsText();
            Controls.Add(layoutPanel);
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

        private Button CreateButton(string name, string text, EventHandler? onClicked) {
            Button button = new() {
                Name = name,
                Text = text
            };
            button.Click += onClicked;
            return button;
        }
    }
}
