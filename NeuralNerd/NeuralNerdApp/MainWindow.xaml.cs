using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Networking;
using Microsoft.Win32;
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
                InputNeuronCount = 4,
                HiddenLayerCount = 2,
                HiddenLayerNeuronCount = 4,
                OutputNeuronCount = 3
            });



            NetworkConfiguration config = new NetworkConfiguration();
            config.Network = network;

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
    }
}
