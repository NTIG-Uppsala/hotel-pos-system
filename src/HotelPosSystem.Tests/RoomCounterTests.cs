using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    public class RoomCounterTests : IDisposable, IClassFixture<ProgramFixture> {
        private readonly UIA3Automation _automation;
        private readonly ProgramFixture _fixture;

        private const string SingleRoomRowAutomationId = "Single Room Row";
        private const string DoubleRoomRowAutomationId = "Double Room Row";
        private const string IncrementButtonAutomationId = "incrementButton";
        private const string DecrementButtonAutomationId = "decrementButton";
        private const string ResetButtonAutomationId = "resetButton";
        private const string RoomTypeCounterTextAutomationId = "roomTypeCounterText";

        public RoomCounterTests(ProgramFixture fixture) {
            _automation = new UIA3Automation();
            _fixture = fixture;
        }

        [Fact]
        public void ShouldIncrementNumber() {
            SetCounter(SingleRoomRowAutomationId, 0);
            AutomationElement singleRoomRow = GetElement(SingleRoomRowAutomationId, GetMainWindow());
            Button incrementButton = GetElement(IncrementButtonAutomationId, singleRoomRow).AsButton();
            Label occupiedRoomsText = GetElement(RoomTypeCounterTextAutomationId, singleRoomRow).AsLabel();

            incrementButton.Click();

            Assert.Equal("Single Room: 1", occupiedRoomsText.Text);
        }

        [Fact]
        public void ShouldDecrementNumberWhenCounterPositive() {
            SetCounter(SingleRoomRowAutomationId, 2);
            AutomationElement singleRoomRow = GetElement(SingleRoomRowAutomationId, GetMainWindow());
            Button decrementButton = GetElement(DecrementButtonAutomationId, singleRoomRow).AsButton();
            Label occupiedRoomsText = GetElement(RoomTypeCounterTextAutomationId, singleRoomRow).AsLabel();

            decrementButton.Click();

            Assert.Equal("Single Room: 1", occupiedRoomsText.Text);
        }

        [Fact]
        public void ShouldNotDecrementNumberWhenCounterZero() {
            SetCounter(SingleRoomRowAutomationId, 0);
            AutomationElement singleRoomRow = GetElement(SingleRoomRowAutomationId, GetMainWindow());
            Button decrementButton = GetElement(DecrementButtonAutomationId, singleRoomRow).AsButton();
            Label occupiedRoomsText = GetElement(RoomTypeCounterTextAutomationId, singleRoomRow).AsLabel();

            decrementButton.Click();

            Assert.Equal("Single Room: 0", occupiedRoomsText.Text);
        }

        [Fact]
        public void ShouldSetCounterToZero() {
            SetCounter(SingleRoomRowAutomationId, 2);
            AutomationElement singleRoomRow = GetElement(SingleRoomRowAutomationId, GetMainWindow());
            Button resetButton = GetElement(ResetButtonAutomationId, singleRoomRow).AsButton();
            Label occupiedRoomsText = GetElement(RoomTypeCounterTextAutomationId, singleRoomRow).AsLabel();

            resetButton.Click();

            Assert.Equal("Single Room: 0", occupiedRoomsText.Text);
        }

        [Fact]
        public void DoubleRoomCounter() {
            AutomationElement? doubleRoomRow = GetElement(DoubleRoomRowAutomationId, GetMainWindow());
            Label doubleRoomCounterText = GetElement(RoomTypeCounterTextAutomationId, doubleRoomRow).AsLabel();

            Button incrementButton = GetElement(IncrementButtonAutomationId, doubleRoomRow).AsButton();
            Button decrementButton = GetElement(DecrementButtonAutomationId, doubleRoomRow).AsButton();
            Button resetButton = GetElement(ResetButtonAutomationId, doubleRoomRow).AsButton();

            SetCounter(DoubleRoomRowAutomationId, 0);

            // Is only used to make sure that it is unaffected
            AutomationElement? singleRoomRow = GetElement(SingleRoomRowAutomationId, GetMainWindow());
            Label singleRoomCounterText = GetElement(RoomTypeCounterTextAutomationId, singleRoomRow).AsLabel();
            SetCounter(SingleRoomRowAutomationId, 1);

            incrementButton.Click();
            incrementButton.Click();
            Assert.Equal("Double Room: 2", doubleRoomCounterText.Text);
            Assert.Equal("Single Room: 1", singleRoomCounterText.Text); // Ensure unaffected

            decrementButton.Click();
            Assert.Equal("Double Room: 1", doubleRoomCounterText.Text);
            Assert.Equal("Single Room: 1", singleRoomCounterText.Text); // Ensure unaffected

            resetButton.Click();
            Assert.Equal("Double Room: 0", doubleRoomCounterText.Text);
            Assert.Equal("Single Room: 1", singleRoomCounterText.Text); // Ensure unaffected
        }

        private void SetCounter(string rowAutomationId, int value) {
            AutomationElement row = GetElement(rowAutomationId, GetMainWindow());
            Button resetButton = GetElement(ResetButtonAutomationId, row).AsButton();
            resetButton.Click();
            Button incrementButton = GetElement(IncrementButtonAutomationId, row).AsButton();
            for (int i = 0; i < value; i++) {
                incrementButton.Click();
            }
        }

        private Window GetMainWindow() {
            return _fixture.Application.GetMainWindow(_automation).AsWindow();
        }

        private AutomationElement GetElement(string automationId, AutomationElement parent) {
            ConditionFactory conditionFactory = _automation.ConditionFactory;
            return parent.FindFirstDescendant(conditionFactory.ByAutomationId(automationId));
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
