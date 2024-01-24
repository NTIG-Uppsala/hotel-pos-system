namespace HotelPosSystem {
    internal static class ControlUtilities {
        internal static (CheckBox, Label) AddCheckBoxWithLabel(Panel container, string checkBoxName, bool checkBoxState, bool checkBoxEnabled, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = Padding.Empty
            };

            Label label = AddLabel(layoutPanel, labelName, labelText);
            CheckBox checkBox = AddCheckBox(layoutPanel, checkBoxName, checkBoxState, checkBoxEnabled);

            container.Controls.Add(layoutPanel);
            return (checkBox, label);
        }

        internal static Label AddLabel(Panel container, string name, string text) {
            Label label = new() {
                Name = name,
                Text = text,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            container.Controls.Add(label);
            return label;
        }

        internal static CheckBox AddCheckBox(Panel container, string name, bool isChecked, bool isEnabled) {
            CheckBox checkBox = new() {
                Name = name,
                Checked = isChecked,
                Enabled = isEnabled,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            container.Controls.Add(checkBox);
            return checkBox;
        }

        internal static (ComboBox, Label) AddComboBoxWithLabel(Panel container, string comboBoxName, object[] comboBoxItems, string comboBoxPlaceholder, int comboBoxWidth, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = Padding.Empty
            };

            Label label = AddLabel(layoutPanel, labelName, labelText);
            ComboBox comboBox = AddComboBox(layoutPanel, comboBoxName, comboBoxItems, comboBoxWidth, comboBoxPlaceholder);

            container.Controls.Add(layoutPanel);
            return (comboBox, label);
        }

        internal static ComboBox AddComboBox(Panel container, string name, object[] items, int width, string placeholder) {
            List<object> itemsWithPlaceholder = items.ToList();
            itemsWithPlaceholder.Insert(0, placeholder);
            ComboBox comboBox = new() {
                Name = name,
                DataSource = itemsWithPlaceholder,
                Width = width,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            container.Controls.Add(comboBox);
            return comboBox;
        }

        internal static (DateTimePicker, Label) AddDatePickerWithLabel(Panel container, string datePickerName, DateTime earliestDate, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = Padding.Empty
            };

            Label label = AddLabel(layoutPanel, labelName, labelText);
            DateTimePicker dateTimePicker = AddDatePicker(layoutPanel, datePickerName, earliestDate);

            container.Controls.Add(layoutPanel);
            return (dateTimePicker, label);
        }

        internal static DateTimePicker AddDatePicker(Panel container, string name, DateTime earliestDate) {
            DateTimePicker datePicker = new() {
                Name = name,
                MinDate = earliestDate,
                Format = DateTimePickerFormat.Short
            };
            container.Controls.Add(datePicker);
            return datePicker;
        }

        internal static (TextBox, Label) AddTextBoxWithLabel(Panel container, string textBoxName, int textBoxWidth, string labelName, string labelText) {
            FlowLayoutPanel layoutPanel = new() {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = Padding.Empty
            };

            Label label = AddLabel(layoutPanel, labelName, labelText);
            TextBox textBox = AddTextBox(layoutPanel, textBoxName, textBoxWidth);

            container.Controls.Add(layoutPanel);
            return (textBox, label);
        }

        internal static TextBox AddTextBox(Panel container, string name, int width) {
            TextBox textBox = new() {
                Name = name,
                Width = width
            };
            container.Controls.Add(textBox);
            return textBox;
        }

        internal static Button AddButton(Panel container, string name, string text, int width, EventHandler onClick) {
            Button button = new() {
                Name = name,
                Text = text,
                Width = width,
                UseCompatibleTextRendering = true,
                AutoSize = true
            };
            button.Click += onClick;
            container.Controls.Add(button);
            return button;
        }

        internal static void CreateRowsAndColumns(TableLayoutPanel table) {
            float rowHeight = 100f / table.RowCount;
            for (int rowIndex = 0; rowIndex < table.RowCount; rowIndex++) {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, rowHeight));
            }

            float columnWidth = 100f / table.ColumnCount;
            for (int columnIndex = 0; columnIndex < table.ColumnCount; columnIndex++) {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnWidth));
            }
        }
    }
}
