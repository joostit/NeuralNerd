using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeuralNerdApp
{
    /// <summary>
    /// Interaction logic for NetworkCanvas.xaml
    /// </summary>
    public partial class NetworkCanvas : UserControl
    {


        private NetworkConfiguration Network { get; set; }

        private Dictionary<NeuronCoordinate, NeuronControl> neurons = new Dictionary<NeuronCoordinate, NeuronControl>();

        public NetworkCanvas()
        {
            InitializeComponent();
        }


        public void Clear()
        {
            foreach (var neuron in neurons.Values)
            {
                neuron.Clear();
            }
            neurons.Clear();

            canvas.Children.Clear();
            Network = null;
        }

        public void SetNetwork(NetworkConfiguration network)
        {
            if(this.Network != null)
            {
                Clear();
            }

            this.Network = network;
            DrawNetwork();
        }

        private void DrawNetwork()
        {
            int y = 10;


            DrawColumn(Network.Network.InputLayer, y);


        }

        private void DrawColumn(InputLayer layer, int y)
        {
            double x = 10;
            foreach(Neuron neuron in layer)
            {
                NeuronControl ctrl = new NeuronControl(neuron);
                
                Canvas.SetLeft(ctrl, y);
                Canvas.SetTop(ctrl, x);
                neurons.Add(neuron.Coordinate, ctrl);
                canvas.Children.Add(ctrl);
                x += NeuronControl.Size + 5;
            }
        }
    }
}
