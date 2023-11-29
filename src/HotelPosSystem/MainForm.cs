
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

            Button incrementButton = new() {
                Name = "incrementButton",
                Text = "+"
            };
            incrementButton.Click += OnIncrementButtonClicked;
            layoutPanel.Controls.Add(incrementButton);

            Button decrementButton = new() {
                Name = "decrementButton",
                Text = "-"
            };
            decrementButton.Click += OnDecrementButtonClicked;
            layoutPanel.Controls.Add(decrementButton);

            Button resetButton = new() {
                Name = "resetButton",
                Text = "Reset"
            };
            resetButton.Click += OnResetButtonClicked;
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
    }
}
