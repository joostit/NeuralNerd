using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Networking;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public MainWindow()
        {

            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            var ci = new CultureInfo(currentCulture) { NumberFormat = { NumberDecimalSeparator = "." } };
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewNetworkBuilder builder = new NewNetworkBuilder();

            NeuralNetwork network = builder.BuildNetwork(new NetworkParameters()
            {
                InputNeuronCount = 6,
                HiddenLayerCount = 2,
                HiddenLayerNeuronCount = 4,
                OutputNeuronCount = 3
            });



            NetworkConfiguration config = new NetworkConfiguration();
            config.Network = network;

            networkCanvas.SetNetwork(config);
        }
    }
}
