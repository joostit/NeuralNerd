using Joostit.NeuralNerd.NnLib.Networking.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
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

        private OutputNeuron outputNeuron = null;

        private List<Dendrite> dendriteEditList = new List<Dendrite>();

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


        private void PopulateDatagrid()
        {
            // Make a shadow collection for the datagrid to edit because of two-way binding
            foreach (var realDendrite in Neuron.Dendrites)
            {
                Dendrite editDendrite = new Dendrite();
                editDendrite.InputNeuronCoordinate = realDendrite.InputNeuronCoordinate.Clone();
                editDendrite.Weight = realDendrite.Weight;
                dendriteEditList.Add(editDendrite);
            }

            dendriteData.ItemsSource = dendriteEditList;
            dendriteData.Columns.Clear();

            var sourceColumn = new DataGridTextColumn();
            sourceColumn.Header = "Source neuron";
            Binding sourceBinding = new Binding("InputNeuronCoordinate");
            sourceBinding.Mode = BindingMode.OneWay;
            sourceColumn.Binding = sourceBinding;
            sourceColumn.IsReadOnly = true;
            dendriteData.Columns.Add(sourceColumn);

            var weightColumn = new DataGridTextColumn();
            weightColumn.Header = "Weight";
            Binding weightBinding = new Binding("Weight");
            weightColumn.Binding = weightBinding;
            weightColumn.IsReadOnly = false;
            dendriteData.Columns.Add(weightColumn);
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            ValuesChanged = true;
            this.DialogResult = true;
        }

        private void SaveData()
        {
            if(outputNeuron != null)
            {
                outputNeuron.Name = nameBox.Text.Trim();
            }

            for(int i = 0; i < dendriteEditList.Count; i++)
            {
                Neuron.Dendrites[i].Weight = dendriteEditList[i].Weight;
            }

            Neuron.Bias = double.Parse(biasBox.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;

            biasBox.Text = Neuron.Bias.ToString();
            PopulateDatagrid();

            outputNeuron = Neuron as OutputNeuron;
            if (outputNeuron != null)
            {
                nameLabel.Visibility = Visibility.Visible;
                nameBox.Visibility = Visibility.Visible;
                nameBox.Text = outputNeuron.Name;
                nameBox.Focus();
                nameBox.SelectAll();
            }
            else
            {
                nameLabel.Visibility = Visibility.Collapsed;
                nameBox.Visibility = Visibility.Collapsed;
                biasBox.Focus();
                biasBox.SelectAll();
            }

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            okButton.IsEnabled = ValidateContent();
        }

        private bool ValidateContent()
        {
            double? value = null;

            try
            {
                value = double.Parse(biasBox.Text);
            }
            catch (FormatException)
            {
                return false;
            }

            if (dendriteData.ItemsSource != null)
            {
                var dataGridErrors = (from c in
                      (from object i in dendriteData.ItemsSource
                       select dendriteData.ItemContainerGenerator.ContainerFromItem(i))
                                      where c != null
                                      select Validation.GetHasError(c))
                 .FirstOrDefault(x => x);

                return !dataGridErrors;
            }
            else
            {
                return false;
            }
        }

        private void dendriteData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(e.EditAction == DataGridEditAction.Commit)
            {
                UpdateButtonState();
            }
        }

        private void biasBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonState();
        }
    }
}
