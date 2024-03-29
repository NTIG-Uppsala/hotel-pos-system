﻿using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;

namespace HotelPosSystem.Tests {
    internal static class Utilities {
        /// <summary>
        /// Finds and returns an element with a specific <paramref name="automationId"/> in the <paramref name="parent"/> element
        /// </summary>
        /// <param name="automationId">The same as <see cref="System.Windows.Forms.Control.Name"/></param>
        /// <param name="parent">The element to start the search from</param>
        /// <returns>The element if found, otherwise <c>null</c></returns>
        internal static AutomationElement GetElement(UIA3Automation automation, string automationId, AutomationElement parent) {
            ConditionFactory conditionFactory = automation.ConditionFactory;
            return parent.FindFirstDescendant(conditionFactory.ByAutomationId(automationId));
        }

        internal static AutomationElement GetBookingListElement(ProgramWithTestDatabase program, UIA3Automation automation) {
            const string bookingListAutomationId = "bookingList";
            return GetElement(automation, bookingListAutomationId, GetMainWindow(program, automation));
        }

        internal static Window GetMainWindow(ProgramWithTestDatabase program, UIA3Automation automation) {
            return program.Application.GetMainWindow(automation).AsWindow();
        }
    }
}
