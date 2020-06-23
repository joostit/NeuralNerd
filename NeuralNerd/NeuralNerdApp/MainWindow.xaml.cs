using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Networking;
using Microsoft.Win32;
using NeuralNerdApp.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeuralNerdApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string FileFilter = "NeuralNet Files (.nn.xml)|*.nn.xml|All files (*.*)|*.*";

        public MainWindow()
        {

            //var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            //var ci = new CultureInfo(currentCulture) { NumberFormat = { NumberDecimalSeparator = "." } };
            //System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            //FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewNetworkBuilder builder = new NewNetworkBuilder();

            NeuralNetwork network = builder.BuildNetwork(new NetworkParameters()
            {
                InputNeuronCount = 16,
                HiddenLayerCount = 2,
                HiddenLayerNeuronCount = 8,
                OutputNeuronCount = 6
            });


            NetworkConfiguration config = new NetworkConfiguration();
            config.Network = network;
            network.InputLayer[0].Activation = 0;
            network.InputLayer[1].Activation = .1;
            network.InputLayer[2].Activation = .2;
            network.InputLayer[3].Activation = .3;
            network.InputLayer[4].Activation = .4;
            network.InputLayer[5].Activation = .5;
            network.InputLayer[6].Activation = .6;
            network.InputLayer[7].Activation = .7;
            network.InputLayer[8].Activation = .8;
            network.InputLayer[9].Activation = .9;
            network.InputLayer[10].Activation = 1;

            networkCanvas.SetNetwork(config);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string filePath = getOpenFilePath();

            if (filePath != null)
            {
                LoadNeuralNetwork(filePath);
            }
        }

        private void LoadNeuralNetwork(string filePath)
        {
            try
            {
                ConfigurationFileHandler fileHandler = new ConfigurationFileHandler();
                NetworkConfiguration config = fileHandler.Load(filePath);
                NeuralNetworkLinker linker = new NeuralNetworkLinker();
                linker.Link(config.Network);
                networkCanvas.SetNetwork(config);

            }catch(Exception e)
            {
                MessageBox.Show($"Error while loading neural network.\n\n{e}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string path = GetSaveAsFilePath();

            if (path != null)
            {
                SaveNeuralNetwork(path);
            }
        }

        private string GetSaveAsFilePath()
        {
            string retVal = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FileFilter;
            saveFileDialog.DefaultExt = "*.nn.xml";
            saveFileDialog.Title = "Save Neural Network as";
            saveFileDialog.FileName = "Neural Network.nn.xml";
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                retVal = saveFileDialog.FileName;
            }

            return retVal;
        }

        private void SaveNeuralNetwork(string path)
        {
            try
            {
                ConfigurationFileHandler fileHandler = new ConfigurationFileHandler();
                fileHandler.Save(networkCanvas.Network, path);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while saving neural network.\n\n{e}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private string getOpenFilePath()
        {
            string result = null;

            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = FileFilter;
            diag.CheckFileExists = true;
            diag.Title = "Select Neural network to open";
            diag.Multiselect = false;

            if (diag.ShowDialog() == true)
            {
                result = diag.FileName;
            }

            return result;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NetworkParameters parameters = NewNetworkWindow.GetNewNetwork();

            if (parameters != null)
            {
                NewNetworkBuilder builder = new NewNetworkBuilder();
                NeuralNetwork network = builder.BuildNetwork(parameters);

                NeuralNetworkLinker linker = new NeuralNetworkLinker();
                linker.Link(network);

                NetworkConfiguration config = new NetworkConfiguration()
                {
                    Network = network
                };
                networkCanvas.SetNetwork(config);
            }
        }
    }
}
