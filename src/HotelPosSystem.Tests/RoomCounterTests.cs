using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    public class RoomCounterTests : IDisposable, IClassFixture<ProgramFixture> {
        private readonly UIA3Automation _automation;
        private readonly ProgramFixture _fixture;

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
            SetCounter(0);
            Button incrementButton = GetElement(IncrementButtonAutomationId).AsButton();
            Label occupiedRoomsText = GetElement(RoomTypeCounterTextAutomationId).AsLabel();

            incrementButton.Click();

            Assert.Equal("Single Room: 1", occupiedRoomsText.Text);
        }

        [Fact]
        public void ShouldDecrementNumberWhenCounterPositive() {
            SetCounter(2);
            Button decrementButton = GetElement(DecrementButtonAutomationId).AsButton();
            Label occupiedRoomsText = GetElement(RoomTypeCounterTextAutomationId).AsLabel();

            decrementButton.Click();

            Assert.Equal("Single Room: 1", occupiedRoomsText.Text);
        }

        [Fact]
        public void ShouldNotDecrementNumberWhenCounterZero() {
            SetCounter(0);
            Button decrementButton = GetElement(DecrementButtonAutomationId).AsButton();
            Label occupiedRoomsText = GetElement(RoomTypeCounterTextAutomationId).AsLabel();

            decrementButton.Click();

            Assert.Equal("Single Room: 0", occupiedRoomsText.Text);
        }

        [Fact]
        public void ShouldSetCounterToZero() {
            SetCounter(2);
            Button resetButton = GetElement(ResetButtonAutomationId).AsButton();
            Label occupiedRoomsText = GetElement(RoomTypeCounterTextAutomationId).AsLabel();

            resetButton.Click();

            Assert.Equal("Single Room: 0", occupiedRoomsText.Text);
        }

        private void SetCounter(int value) {
            Button resetButton = GetElement(ResetButtonAutomationId).AsButton();
            resetButton.Click();
            Button incrementButton = GetElement(IncrementButtonAutomationId).AsButton();
            for (int i = 0; i < value; i++) {
                incrementButton.Click();
            }
        }

        private AutomationElement GetElement(string automationId) {
            Window mainWindow = _fixture.Application.GetMainWindow(_automation).AsWindow();
            ConditionFactory conditionFactory = _automation.ConditionFactory;
            return mainWindow.FindFirstDescendant(conditionFactory.ByAutomationId(automationId));
        }

        public void Dispose() {
            _automation.Dispose();
        }
    }
}
