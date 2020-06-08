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

        private double maxX = 0;
        private double maxY = 0;


        public NetworkCanvas()
        {
            InitializeComponent();
        }


        #region Zooming and panning

        /// <summary>
        /// The point that was clicked relative to the ZoomAndPanControl.
        /// </summary>
        private Point origZoomAndPanControlMouseDownPoint;

        /// <summary>
        /// The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
        /// </summary>
        private Point origContentMouseDownPoint;

        /// <summary>
        /// Records which mouse button clicked during mouse dragging.
        /// </summary>
        private MouseButton mouseButtonDown;

        /// <summary>
        /// Specifies the current state of the mouse handling logic.
        /// </summary>
        private MouseHandlingMode mouseHandlingMode = MouseHandlingMode.None;

        /// <summary>
        /// Event raised on mouse down in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            canvas.Focus();
            Keyboard.Focus(canvas);

            mouseButtonDown = e.ChangedButton;
            origZoomAndPanControlMouseDownPoint = e.GetPosition(zoomAndPanControl);
            origContentMouseDownPoint = e.GetPosition(canvas);

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 &&
                (e.ChangedButton == MouseButton.Left ||
                 e.ChangedButton == MouseButton.Right))
            {
                // Shift + left- or right-down initiates zooming mode.
                mouseHandlingMode = MouseHandlingMode.Zooming;
            }
            else if (mouseButtonDown == MouseButton.Left)
            {
                // Just a plain old left-down initiates panning mode.
                mouseHandlingMode = MouseHandlingMode.Panning;
            }

            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                // Capture the mouse so that we eventually receive the mouse up event.
                zoomAndPanControl.CaptureMouse();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse up in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                if (mouseHandlingMode == MouseHandlingMode.Zooming)
                {
                    if (mouseButtonDown == MouseButton.Left)
                    {
                        // Shift + left-click zooms in on the content.
                        ZoomIn();
                    }
                    else if (mouseButtonDown == MouseButton.Right)
                    {
                        // Shift + left-click zooms out from the content.
                        ZoomOut();
                    }
                }

                zoomAndPanControl.ReleaseMouseCapture();
                mouseHandlingMode = MouseHandlingMode.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse move in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseHandlingMode == MouseHandlingMode.Panning)
            {
                //
                // The user is left-dragging the mouse.
                // Pan the viewport by the appropriate amount.
                //
                Point curContentMousePoint = e.GetPosition(canvas);
                Vector dragOffset = curContentMousePoint - origContentMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= dragOffset.X;
                zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised by rotating the mouse wheel
        /// </summary>
        private void zoomAndPanControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                ZoomIn();
            }
            else if (e.Delta < 0)
            {
                ZoomOut();
            }
        }


        /// <summary>
        /// Zoom the viewport out by a small increment.
        /// </summary>
        private void ZoomOut()
        {
            zoomAndPanControl.ContentScale -= 0.1;
        }

        /// <summary>
        /// Zoom the viewport in by a small increment.
        /// </summary>
        private void ZoomIn()
        {
            zoomAndPanControl.ContentScale += 0.1;
        }

        #endregion



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
            maxX = 0;
            maxY = 0;

            double x = 10;
            DrawInputColumn(Network.Network.InputLayer, ref x);

            foreach (var hiddenLayer in Network.Network.HiddenLayers)
            {
                DrawHiddenLayer(hiddenLayer, ref x);
            }

            DrawOutputLayer(Network.Network.OutputLayer, ref x);
            MoveDendritesBehindNeurons();

            ResizeCanvas();
        }

        private void ResizeCanvas()
        {
            canvas.Width = maxX + 50;
            canvas.Height = maxY + 50;
        }

        private void MoveDendritesBehindNeurons()
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

        private void DrawOutputLayer(OutputLayer outputLayer, ref double x)
        {
            double y = 10;
            foreach (CalculatedNeuron calculatedNeuron in outputLayer)
            {
                DrawCalculatedNeuron(calculatedNeuron, x, y);
                y += NeuronControl.Size + 5;
            }
            x += LayerWidth;

            // ToDo: Determine if the new Y should be larger if we're adding a name to output neurons

            UpdateMaxSize(x,y);
        }

        private void DrawHiddenLayer(HiddenLayer hiddenLayer, ref double x)
        {
            double y = 10;
            foreach (CalculatedNeuron neuron in hiddenLayer)
            {
                DrawCalculatedNeuron(neuron, x, y);
                y += NeuronControl.Size + 5;
            }
            x += LayerWidth;

            UpdateMaxSize(x, y);
        }


        private void DrawCalculatedNeuron(CalculatedNeuron neuron, double x, double y)
        {
            CalculatedNeuronControl ctrl = new CalculatedNeuronControl(neuron);

            Canvas.SetLeft(ctrl, x);
            Canvas.SetTop(ctrl, y);
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


        private void DrawInputColumn(InputLayer layer, ref double x)
        {
            double y = 10;
            foreach(InputNeuron neuron in layer)
            {
                InputNeuronControl ctrl = new InputNeuronControl(neuron);
                ctrl.ActivationChanged += InputNeuron_ActivationChanged;
                Canvas.SetLeft(ctrl, x);
                Canvas.SetTop(ctrl, y);
                neurons.Add(neuron.Coordinate, ctrl);
                canvas.Children.Add(ctrl);
                y += NeuronControl.Size + 5;
            }
            x += LayerWidth;

            UpdateMaxSize(x, y);
        }

        private void UpdateMaxSize(double x, double y)
        {
            if (x > maxX)
            {
                maxX = x;
            }

            if(y > maxY)
            {
                maxY = y;
            }
        }

        private void InputNeuron_ActivationChanged(object sender, EventArgs e)
        {
            // ToDo: Recalculate the network
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
