using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Learning;
using Joostit.NeuralNerd.NnLib.Networking;
using Microsoft.Win32;
using NeuralNerdApp.Windows;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace NeuralNerdApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer updateTimer = new DispatcherTimer();
        private TimeSpan updateInterval = new TimeSpan(0, 0, 0, 0, 500);

        private NetworkUiContext networkContext;

        private const string NeuralNetworkFileFilter = "NeuralNet Files (.nn.xml)|*.nn.xml|All files (*.*)|*.*";
        private const string ImageFileFilter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

        public MainWindow()
        {
            
            InitializeComponent();

            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = updateInterval;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            networkContext = new NetworkUiContext();
            networkPerformanceControl.SetNetworkContext(networkContext);
            networkCanvas.NetworkContext = networkContext;

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
            network.InputLayer.Neurons[0].Activation = 0;
            network.InputLayer.Neurons[1].Activation = .1;
            network.InputLayer.Neurons[2].Activation = .2;
            network.InputLayer.Neurons[3].Activation = .3;
            network.InputLayer.Neurons[4].Activation = .4;
            network.InputLayer.Neurons[5].Activation = .5;
            network.InputLayer.Neurons[6].Activation = .6;
            network.InputLayer.Neurons[7].Activation = .7;
            network.InputLayer.Neurons[8].Activation = .8;
            network.InputLayer.Neurons[9].Activation = .9;
            network.InputLayer.Neurons[10].Activation = 1;

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
            saveFileDialog.Filter = NeuralNetworkFileFilter;
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
                fileHandler.Save(networkCanvas.NetworkContext.NetworkConfig, path);
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
            diag.Filter = NeuralNetworkFileFilter;
            diag.CheckFileExists = true;
            diag.Title = "Select Neural network to open";
            diag.Multiselect = false;

            if (diag.ShowDialog() == true)
            {
                result = diag.FileName;
            }

            return result;
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
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

        private async void RandomizeLearning_Click(object sender, RoutedEventArgs e)
        {
            await networkCanvas.InitializeLearning();
        }

        private async void Learn_Click(object sender, RoutedEventArgs e)
        {
            var diag = new VistaFolderBrowserDialog();
            diag.Description = "Select stimuli root folder";
            if (diag.ShowDialog() == true)
            {
                updateTimer.Start();
                await networkCanvas.Learn(diag.SelectedPath);
                //updateTimer.Stop();

                //networkPerformanceControl.UpdateLearningState(null, false);
                //networkCanvas.UpdateLearningState(null);

                //// Manually update the learning state once
                //UpdateTimer_Tick(this, null);
            }
        }

        BackgroundWorker worker;

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (networkContext.Learner != null)
            {
                NetworkSnapshot pass = new NetworkSnapshot();

                var task = Task.Run(async () => { await pass.Load(networkContext.Learner); });
                task.Wait();

                e.Result = pass;
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            NetworkSnapshot pass = e.Result as NetworkSnapshot;
            networkPerformanceControl.UpdateLearningState(pass, true);
            networkCanvas.UpdateLearningState(pass);
        }

        private void OpenStimulus_Click(object sender, RoutedEventArgs e)
        {
            string path = getSingleStimulusFilePath();

            if(path != null)
            {
                networkCanvas.LoadSingleStimulus(path);

                NetworkSnapshot pass = new NetworkSnapshot();

                var task = Task.Run(async () => { await pass.Load(networkContext.Learner); });
                task.Wait();

                networkPerformanceControl.UpdateLearningState(pass, false);
                networkCanvas.UpdateLearningState(pass);
                networkCanvas.SetIdleMode();
            }
        }

        private string getSingleStimulusFilePath()
        {
            string result = null;

            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = ImageFileFilter;
            diag.CheckFileExists = true;
            diag.Title = "Select image";
            diag.Multiselect = false;

            if (diag.ShowDialog() == true)
            {
                result = diag.FileName;
            }

            return result;
        }

        private void networkCanvas_NetworkStatusChanged(object sender, EventArgs e)
        {
            NetworkSnapshot pass = new NetworkSnapshot();

            var task = Task.Run(async () => { await pass.Load(networkContext.Learner); });
            task.Wait();

            networkPerformanceControl.UpdateValues(pass);
        }

    }
}
