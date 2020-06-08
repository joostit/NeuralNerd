using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NeuralNerdApp.Forms
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NumericInputWindow : Window
    {


        public double Min { get; set; }
        public double Max { get; set; }

        public double? Value { get; set; }

        private NumericInputWindow()
        {
            InitializeComponent();
        }


        public static double? Show(double min, double max, double initialValue)
        {
            double? retVal = null;

            NumericInputWindow window = new NumericInputWindow();
            window.Min = min;
            window.Max = max;
            window.Value = initialValue;
            
            if(window.ShowDialog() == true)
            {
                retVal = window.Value;
            }

            return retVal;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Value = double.Parse(inputBox.Text.Trim());
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;

            if (Value != null)
            {
                inputBox.Text = Value.Value.ToString();
            }
            UpdateButtonState();
            inputBox.Focus();
            inputBox.SelectAll();
        }

        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonState();
        }

       
        private bool UpdateButtonState()
        {
            bool isValid = ValidateContent();
            okButton.IsEnabled = isValid;
            return isValid;
        }

        private bool ValidateContent()
        {
            double? value = null;

            try
            {
                value = double.Parse(inputBox.Text);
            }
            catch (FormatException)
            {
                return false;
            }

            if(value < Min || value > Max)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
