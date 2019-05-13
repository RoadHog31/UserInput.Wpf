using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UserInput.Wpf
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Gets or sets the value of the first number that the arithmetic will be performed on. 
        private double? FirstNumber { get; set; }

        //Gets or sets the value of the second number that the arithmetic will be performed on. 
        private double? SecondNumber { get; set; }

        // Gets or sets the selected arithmetic operation. 
        private Func<double, double, double> SelectedMathFunction { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        // This method accepts two number parameters "a" and "b", and returns one number "result"
        private double Add(double a, double b)
        {
            double result = a + b;
            return result;
        }

        // This method accepts two number parameters "a" and "b", and returns one number "result"
        private double Subtract(double a, double b)
        {
            double result = a - b;
            return result;
        }

        // This method accepts two number parameters "a" and "b", and returns one number "result"
        private double Multiply(double a, double b)
        {
            double result = a * b;
            return result;
        }

        // This method accepts two number parameters "a" and "b", and returns one number "result"
        private double Divide(double a, double b)
        {
            double result = a / b;
            return result;
        }

        private void FirstNumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check if the text in FirstNumberBox.Text is empty.
            if (string.IsNullOrEmpty(FirstNumberBox?.Text))
            {
                FirstNumber = null;
                return;
            }

            // If text was entered, check to see if it can be parsed into an number.
            if (double.TryParse(FirstNumberBox?.Text, out double parsedNumber))
            {
                // If the text is an integer, then set the value of the FirstNumber property.
                FirstNumber = parsedNumber;
            }
            else
            {
                // If it not a number, remove the last entered character with Trim() method.
                FirstNumberBox.Text = FirstNumberBox.Text.TrimEnd(FirstNumberBox.Text.LastOrDefault());
            }
        }

        // We can share one event handler for all the RadioButtons' Checked event, this prevents a lot of duplicate code.
        private void RadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            // We can use a single line of code to get the RadioButton reference.
            var radioButton = sender as RadioButton;

            // Get the value of the RadioButton's Content (this contains the name of the math operation we want).
            string radioButtonContent = radioButton?.Content?.ToString();

            // Set the appropriate arithmetic operation to use by checking radioButtonContent's value.
            switch (radioButtonContent)
            {
                case "Add": SelectedMathFunction = Add;
                    break;

                case "Subtract": SelectedMathFunction = Subtract;
                    break;

                case "Multiply": SelectedMathFunction = Multiply;
                    break;

                case "Divide": SelectedMathFunction = Divide;
                    break;

                default:
                    SelectedMathFunction = null;
                    break;
            }
        }

        private void SecondNumberBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            // We can reuse the same logic we did for the FirstNumberBox.
            if (string.IsNullOrEmpty(SecondNumberBox?.Text))
            {
                SecondNumber = null;
                return;
            }

            if (double.TryParse(SecondNumberBox?.Text, out double parseNumber))
            {
                SecondNumber = parseNumber;
            }
            else
            {
                SecondNumberBox.Text = SecondNumberBox.Text.TrimEnd(SecondNumberBox.Text.LastOrDefault());
            }

        }

        private void SecondNumberSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // You would typically use e.NewValue to get the Slider's value as it changes.
            // However, the SecondTextBox.TextChanged event is already getting the value, we can just set the SecondNumberBox.Text instead.
            SecondNumberBox.Text = e.NewValue.ToString("N");
        }

        private void IncludeDateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // When the CheckBox is checked, show the DatePickers.
            CalculationDatePicker.Visibility = Visibility.Visible;
            CalculationDatePicker.SelectedDate = DateTime.Now;
        }

        private void IncludeDateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // When the CheckBox is unchecked, hide the DatePickers.
            CalculationDatePicker.Visibility = Visibility.Collapsed;
        }

        private void EqualsButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Before doing any calculations, confirm the user entered both numbers.
            if (FirstNumber == null || SecondNumber == null)
            {
                MessageBox.Show("You need to set both the numbers to calculate a result.");
                return;
            }

            // Now is a good time to do some validation on the numbers and prevent any serious problems.
            // For example, here we make sure the user isn't trying to divide from zero, this can crash your app!
            if (SecondNumber == 0 && SelectedMathFunction == Divide)
            {
                MessageBox.Show("You cannot divide from zero, please pick a different 2nd number.");
                return;
            }

            // Next, it's time to do the actual math. We only need to pass the two numbers into the SelectedMathFunction.
            double result = SelectedMathFunction((double)FirstNumber, (double)SecondNumber);

            // Finally, show the result to the user!
            if (IncludeDateCheckBox.IsChecked == true)
            {
                // If the CheckBox was checked, show the number with the "N2" string format (a number to 2 decimal points),
                // but also include the Date in the output with the "d" string format (a short date format).
                ResultsTextBlock.Text = $"Result: {result:N2}, Date: {CalculationDatePicker.SelectedDate:d}";
            }
        }
    }
}
