using Joostit.NeuralNerd.NnLib.Networking.Elements;
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
    /// Interaction logic for CalculatedNeuronEditWindow.xaml
    /// </summary>
    public partial class CalculatedNeuronEditWindow : Window
    {

        public CalculatedNeuron Neuron { get; set; }

        public bool? ValuesChanged { get; set; }

        public CalculatedNeuronEditWindow()
        {
            InitializeComponent();
        }


        public static bool Show(CalculatedNeuron neuron)
        {
            bool retVal = false;

            CalculatedNeuronEditWindow window = new CalculatedNeuronEditWindow();
            window.Neuron = neuron;

            if (window.ShowDialog() == true)
            {
                if(window.ValuesChanged != null)
                {
                    retVal = window.ValuesChanged.Value;
                }
            }

            return retVal;
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            ValuesChanged = true;
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;

            
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            
        }
    }
}
