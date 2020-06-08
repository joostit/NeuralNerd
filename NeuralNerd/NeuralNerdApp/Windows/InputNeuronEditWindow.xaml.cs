using Joostit.NeuralNerd.NnLib.Networking.Elements;
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

    public partial class InputNeuronEditWindow : Window
    {

        public InputNeuron Neuron { get; set; }

        public double Min { get; set; } = 0;
        public double Max { get; set; } = 1;

        public double? Value { get; set; }

        private InputNeuronEditWindow()
        {
            InitializeComponent();
        }


        public static bool Show(InputNeuron neuron)
        {
            bool retVal = false;

            InputNeuronEditWindow window = new InputNeuronEditWindow();
            window.Neuron = neuron;
            
            if(window.ShowDialog() == true)
            {
                retVal = true;
            }

            return retVal;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Value = double.Parse(activationBox.Text.Trim());

            Neuron.Activation = Value.Value;
            Neuron.Name = nameBox.Text.Trim();

            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;

            if(Neuron == null)
            {
                throw new InvalidOperationException("The Neuron Property needs to be set before showing this window");
            }

            Value = Neuron.Activation;
            activationBox.Text = Value.Value.ToString();
            nameBox.Text = Neuron.Name;

            UpdateButtonState();
            activationBox.Focus();
            activationBox.SelectAll();
        }

        private void activationBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (activationBox.IsLoaded)
            {
                UpdateButtonState();
            }
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
                value = double.Parse(activationBox.Text);
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
