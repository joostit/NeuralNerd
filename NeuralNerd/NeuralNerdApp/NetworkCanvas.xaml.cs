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

        private const double LayerWidth = NeuronControl.Size * 3;

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
            double y = 10;

            DrawInputColumn(Network.Network.InputLayer, y);
            y += LayerWidth;

            foreach (var hiddenLayer in Network.Network.HiddenLayers)
            {
                DrawHiddenLayer(hiddenLayer, y);
                y += LayerWidth;
            }

            DrawOutputLayer(Network.Network.OutputLayer, y);

            DrawDendritesBehindNeurons();

        }

        private void DrawDendritesBehindNeurons()
        {
            foreach(UIElement child in canvas.Children)
            {
                if(child is NeuronControl)
                {
                    Panel.SetZIndex(child, 99999999);
                }
            }
            canvas.UpdateLayout();
        }

        private void DrawOutputLayer(OutputLayer outputLayer, double y)
        {
            double x = 10;
            foreach (CalculatedNeuron calculatedNeuron in outputLayer)
            {
                DrawCalculatedNeuron(calculatedNeuron, x, y);
                x += NeuronControl.Size + 5;
            }
        }

        private void DrawHiddenLayer(HiddenLayer hiddenLayer, double y)
        {
            double x = 10;
            foreach (CalculatedNeuron neuron in hiddenLayer)
            {
                DrawCalculatedNeuron(neuron, x, y);
                x += NeuronControl.Size + 5;
            }
        }


        private void DrawCalculatedNeuron(CalculatedNeuron neuron, double x, double y)
        {
            NeuronControl ctrl = new NeuronControl(neuron);

            Canvas.SetLeft(ctrl, y);
            Canvas.SetTop(ctrl, x);
            neurons.Add(neuron.Coordinate, ctrl);
            canvas.Children.Add(ctrl);

            DrawDendrites(neuron, ctrl);
        }


        private void DrawDendrites(CalculatedNeuron targetNeuron, NeuronControl targetNeuronCtrl)
        {
            foreach(Dendrite dendrite in targetNeuron.Dendrites)
            {
                NeuronControl sourceNeuron = neurons[dendrite.InputNeuronCoordinate];
                DendriteControl dendriteCtrl = new DendriteControl(dendrite, sourceNeuron.Neuron, targetNeuron);

                dendriteCtrl.X1 = Canvas.GetLeft(sourceNeuron) + (NeuronControl.Size / 2);
                dendriteCtrl.Y1 = Canvas.GetTop(sourceNeuron) + (NeuronControl.Size / 2);

                dendriteCtrl.X2 = Canvas.GetLeft(targetNeuronCtrl) + (NeuronControl.Size / 2);
                dendriteCtrl.Y2 = Canvas.GetTop(targetNeuronCtrl) + (NeuronControl.Size / 2);

                canvas.Children.Add(dendriteCtrl);
                
            }
        }


        private void DrawInputColumn(InputLayer layer, double y)
        {
            double x = 10;
            foreach(InputNeuron neuron in layer)
            {
                InputNeuronControl ctrl = new InputNeuronControl(neuron);
                
                Canvas.SetLeft(ctrl, y);
                Canvas.SetTop(ctrl, x);
                neurons.Add(neuron.Coordinate, ctrl);
                canvas.Children.Add(ctrl);
                x += NeuronControl.Size + 5;
            }
        }
    }
}
