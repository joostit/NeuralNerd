using Joostit.NeuralNerd.NnLib.Construction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NeuralNerdApp.Windows
{
    /// <summary>
    /// Interaction logic for NewNetworkWindow.xaml
    /// </summary>
    public partial class NewNetworkWindow : Window
    {

        private NetworkParameters Result;

        public NewNetworkWindow()
        {
            InitializeComponent();
        }


        public static NetworkParameters GetNewNetwork()
        {
            NetworkParameters retVal = null;

            NewNetworkWindow window = new NewNetworkWindow();

            if (window.ShowDialog() == true)
            {
                retVal = window.Result;
            }

            return retVal;
        }


        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            okButton.IsEnabled = ValidateContent();
        }

        private bool ValidateContent()
        {
            int nInputs;
            int nHiddenL;
            int nHiddenN;
            int nOutputs;

            try
            {
                nInputs = int.Parse(inputNeuronBox.Text.Trim());
                nHiddenL = int.Parse(hiddenLayerCountBox.Text.Trim());
                nHiddenN = int.Parse(hiddenLayerSizeBox.Text.Trim());
                nOutputs = int.Parse(outputNeuronsBox.Text.Trim());
            }
            catch (FormatException)
            {
                return false;
            }

            if (nInputs < 1) return false;
            if (nHiddenL < 1) return false;
            if (nHiddenN < 1) return false;
            if (nOutputs < 1) return false;

            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;

            inputNeuronBox.Text = "16";
            hiddenLayerCountBox.Text = "2";
            hiddenLayerSizeBox.Text = "8";
            outputNeuronsBox.Text = "6";

            UpdateButtonState();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            this.DialogResult = true;
        }

        private void SaveData()
        {
            Result = new NetworkParameters()
            {
                InputNeuronCount = int.Parse(inputNeuronBox.Text.Trim()),
                HiddenLayerCount = int.Parse(hiddenLayerCountBox.Text.Trim()),
                HiddenLayerNeuronCount = int.Parse(hiddenLayerSizeBox.Text.Trim()),
                OutputNeuronCount = int.Parse(outputNeuronsBox.Text.Trim())
            };
        }
    }
}
