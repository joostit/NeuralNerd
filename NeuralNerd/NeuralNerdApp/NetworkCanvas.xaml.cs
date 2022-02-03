using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Learning;
using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        
        private const double InputLayerWidth = InputNeuronControl.InputNeuronWidth + NeuronControl.Size * 3;
        private const double HiddenLayerWidth = NeuronControl.Size * 4;
        private const double OutputLayerWidth = OutputNeuronControl.OutputNeuronWidth + NeuronControl.Size * 4;
        private const double NeuronSpacing = 10;

        private const int MaxDendritesPerLayer = 512;
        public event EventHandler NetworkStatusChanged;

        public NetworkUiContext NetworkContext { get; set; }
        private Dictionary<NeuronCoordinate, NeuronControl> neurons = new Dictionary<NeuronCoordinate, NeuronControl>();

        private double maxX = 0;

        public NetworkCanvas()
        {
            InitializeComponent();
            canvas.Width = 100000000;
            canvas.Height = 100000000;
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
            NetworkContext.NetworkConfig = null;
        }

        public async Task InitializeLearning()
        {
            NetworkContext.Learner.RandomizeNeuronParameters();
            await Calculate();
        }

        internal async Task Learn(string stimulibasePath)
        {
            SetLearnMode();
            NetworkContext.State = OperationalStates.LoadingStimuli;
            await NetworkContext.Learner.LoadStimuliAsync(stimulibasePath);

            NetworkContext.State = OperationalStates.Learning;
            await NetworkContext.Learner.LearnAsync();
        }

        public async void SetNetwork(NetworkConfiguration network)
        {
            if(this.NetworkContext.NetworkConfig != null)
            {
                Clear();
            }

            this.NetworkContext.NetworkConfig = network;
            NetworkContext.Learner = new ImageLearner(network.Network);
            DrawNetwork();
            await Calculate();
            SetIdleMode();
        }


        internal void UpdateLearningState(NetworkSnapshot snapshot)
        {

            if(snapshot != null && snapshot.Pass != null)
            {
                UpdateNeuronLearningState(snapshot);
            }
            else
            {
                SetIdleMode();
            }
        }


        private void UpdateNeuronLearningState(NetworkSnapshot snapshot)
        {
            int layerIndex = 0;

            for(int row = 0; row < snapshot.Pass.State.InputLayerActivations.Length; row++)
            {
                neurons[new NeuronCoordinate(layerIndex, row)].SetLearningActivationState(snapshot.Pass.State.InputLayerActivations[row]);
            }
            layerIndex++;

            foreach (var hiddenLayer in snapshot.Pass.State.HiddenLayersActivations)
            {
                for (int row = 0; row < hiddenLayer.Length; row++)
                {
                    neurons[new NeuronCoordinate(layerIndex, row)].SetLearningActivationState(hiddenLayer[row]);
                }
                layerIndex++;
            }

            for (int row = 0; row < snapshot.Pass.State.OutputLayerActivations.Length; row++)
            {
                neurons[new NeuronCoordinate(layerIndex, row)].SetLearningActivationState(snapshot.Pass.State.OutputLayerActivations[row]);
            }
        }

        internal void LoadSingleStimulus(string path)
        {
            NetworkContext.Learner.LoadSingleStimulus(path);
        }

        public void SetIdleMode()
        {
            foreach(var neuron in neurons.Values)
            {
                neuron.SetEditable(true);
            }
        }

        public void SetLearnMode()
        {
            foreach (var neuron in neurons.Values)
            {
                neuron.SetEditable(false);
            }
        }

        private void DrawNetwork()
        {
            maxX = 0;
            INeuronLayer leftLayer = NetworkContext.NetworkConfig.Network.InputLayer;
            double x = 10;
            DrawInputColumn(NetworkContext.NetworkConfig.Network.InputLayer, ref x);

            foreach (var hiddenLayer in NetworkContext.NetworkConfig.Network.HiddenLayers)
            {
                DendriteDrawModes dendriteMode = (GetDendriteCount(hiddenLayer) <= MaxDendritesPerLayer) ? DendriteDrawModes.All : DendriteDrawModes.None;

                DrawHiddenLayer(hiddenLayer, ref x, dendriteMode);

                if (dendriteMode != DendriteDrawModes.All)
                {
                    DrawCommonDendriteIndicator(leftLayer, hiddenLayer);
                }
                leftLayer = hiddenLayer;
            }

            DrawOutputLayer(NetworkContext.NetworkConfig.Network.OutputLayer, ref x);
            MoveDendritesBehindNeurons();

            ResizeCanvas();
        }


        private double GetMaxY()
        {
            int largestColumnCount = NetworkContext.NetworkConfig.Network.InputLayer.Count;

            foreach (var hiddenLayer in NetworkContext.NetworkConfig.Network.HiddenLayers)
            {
                largestColumnCount = hiddenLayer.Count > largestColumnCount ? hiddenLayer.Count : largestColumnCount;
            }

            largestColumnCount = NetworkContext.NetworkConfig.Network.OutputLayer.Count > largestColumnCount ? NetworkContext.NetworkConfig.Network.OutputLayer.Count : largestColumnCount;

            double maxY = largestColumnCount * (NeuronControl.Size + NeuronSpacing);
            return maxY;
        }


        private double GetColumnStartY(int columnCount)
        {
            double singleNeuronYSize = NeuronControl.Size + NeuronSpacing;
            double totalNeuronYSize = singleNeuronYSize * columnCount;
            double yStart = (GetMaxY() - totalNeuronYSize) / 2.5;
            return yStart;
        }


        private void ResizeCanvas()
        {
            canvas.Width = maxX + 50;
            canvas.Height = GetMaxY() + 50;
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
            DendriteDrawModes dendriteMode = (GetDendriteCount(outputLayer) <= MaxDendritesPerLayer) ? DendriteDrawModes.All : DendriteDrawModes.None;
            double y = 10 + GetColumnStartY(outputLayer.Count);
            foreach (OutputNeuron outputNeuron in outputLayer.Neurons)
            {
                DrawOutputNeuron(outputNeuron, x, y, dendriteMode);
                y += NeuronControl.Size + NeuronSpacing;
            }
            x += OutputLayerWidth + OutputNeuronControl.OutputNeuronWidth;

            UpdateMaxSize(x);

            if (dendriteMode != DendriteDrawModes.All)
            {
                INeuronLayer lastHiddenLayer = NetworkContext.NetworkConfig.Network.HiddenLayers[NetworkContext.NetworkConfig.Network.HiddenLayers.Count - 1];
                DrawCommonDendriteIndicator(lastHiddenLayer, outputLayer);
            }
        }

        private void DrawHiddenLayer(HiddenLayer hiddenLayer, ref double x, DendriteDrawModes dendriteMode)
        {
            double y = 10 + GetColumnStartY(hiddenLayer.Count);
            for(int i = 0; i < hiddenLayer.Neurons.Length; i++)
            {
                CalculatedNeuron neuron = hiddenLayer.Neurons[i];
                DrawHiddenNeuron(neuron, x, y, dendriteMode);
                y += NeuronControl.Size + NeuronSpacing;
            }

            x += HiddenLayerWidth;

            UpdateMaxSize(x);
        }


        private void DrawHiddenNeuron(CalculatedNeuron neuron, double x, double y, DendriteDrawModes dendriteMode)
        {
            CalculatedNeuronControl ctrl = new CalculatedNeuronControl(neuron);
            ctrl.ConfigurationChanged += Ctrl_ConfigurationChanged;
            Canvas.SetLeft(ctrl, x);
            Canvas.SetTop(ctrl, y);
            neurons.Add(neuron.Coordinate, ctrl);
            canvas.Children.Add(ctrl);
            
            DrawDendrites(ctrl, dendriteMode);

        }


        private void DrawOutputNeuron(OutputNeuron neuron, double x, double y, DendriteDrawModes dendriteMode)
        {
            OutputNeuronControl ctrl = new OutputNeuronControl(neuron);
            ctrl.ConfigurationChanged += Ctrl_ConfigurationChanged;
            Canvas.SetLeft(ctrl, x);
            Canvas.SetTop(ctrl, y);
            neurons.Add(neuron.Coordinate, ctrl);
            canvas.Children.Add(ctrl);

            DrawDendrites(ctrl, dendriteMode);
        }


        private void DrawDendrites(CalculatedNeuronControl targetNeuronCtrl, DendriteDrawModes dendriteMode)
        {
            if (dendriteMode == DendriteDrawModes.All)
            {
                foreach(var dendrite in targetNeuronCtrl.Neuron.Dendrites)
                {
                    DrawDendrite(targetNeuronCtrl, dendrite);
                }
            }
        }

        private void DrawDendrite(CalculatedNeuronControl targetNeuronCtrl, Dendrite dendrite)
        {
            NeuronControl sourceNeuron = neurons[dendrite.InputNeuronCoordinate];
            DendriteControl dendriteCtrl = new DendriteControl(dendrite, sourceNeuron.Neuron, targetNeuronCtrl.Neuron);

            dendriteCtrl.X1 = Canvas.GetLeft(sourceNeuron) + sourceNeuron.GetCenterX();
            dendriteCtrl.Y1 = Canvas.GetTop(sourceNeuron) + sourceNeuron.GetCenterY();

            dendriteCtrl.X2 = Canvas.GetLeft(targetNeuronCtrl) + targetNeuronCtrl.GetCenterX();
            dendriteCtrl.Y2 = Canvas.GetTop(targetNeuronCtrl) + targetNeuronCtrl.GetCenterY();

            canvas.Children.Add(dendriteCtrl);
        }

        private void DrawInputColumn(InputLayer layer, ref double x)
        {
            double y = 10 + GetColumnStartY(layer.Count);
            foreach(InputNeuron neuron in layer.Neurons)
            {
                InputNeuronControl ctrl = new InputNeuronControl(neuron);
                ctrl.ActivationChanged += InputNeuron_ActivationChanged;
                Canvas.SetLeft(ctrl, x);
                Canvas.SetTop(ctrl, y);
                neurons.Add(neuron.Coordinate, ctrl);
                canvas.Children.Add(ctrl);
                y += NeuronControl.Size + NeuronSpacing;
            }
            x += InputLayerWidth;

            UpdateMaxSize(x);
        }


        private int GetDendriteCount(ICalculatableNeuronLayer layer)
        {
            int count = 0;

            foreach (CalculatedNeuron neuron in layer.Neurons)
            {
                count += neuron.Dendrites.Length;
            }

            return count;
        }

        private void UpdateMaxSize(double x)
        {
            if (x > maxX)
            {
                maxX = x;
            }
        }

        private async void InputNeuron_ActivationChanged(object sender, EventArgs e)
        {
            await Calculate();
        }

        private async void Ctrl_ConfigurationChanged(object sender, EventArgs e)
        {
            // ToDo: Recalculate the network
            await Calculate();
        }

        public async Task Calculate()
        {
            await Task.Run(() =>
            {
                NetworkContext.NetworkConfig.Network.Calculate();
                NetworkContext.Learner.Learn(1);
            });

            NetworkStatusChanged.Invoke(this, EventArgs.Empty);

            UpdateView();
        }

        

        public void UpdateView()
        {
            foreach (NeuronControl ctrl in neurons.Values)
            {
                ctrl.Update();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void DrawCommonDendriteIndicator(INeuronLayer leftLayer, INeuronLayer rightLayer)
        {
            double x;
            double y;
            PointCollection points = new PointCollection();

            NeuronControl LeftUpper = neurons[leftLayer[0].Coordinate];
            NeuronControl LeftLower = neurons[leftLayer[leftLayer.Count-1].Coordinate];

            NeuronControl rightUpper = neurons[rightLayer[0].Coordinate];
            NeuronControl rightLower = neurons[rightLayer[rightLayer.Count - 1].Coordinate];

            x = Canvas.GetLeft(LeftUpper) + LeftUpper.GetCenterX();
            y = Canvas.GetTop(LeftUpper) + LeftUpper.GetCenterY();
            Point leftUpperPoint = new Point(x, y);
            points.Add(new Point(x, y));

            x = Canvas.GetLeft(rightUpper) + rightUpper.GetCenterX();
            y = Canvas.GetTop(rightUpper) + rightUpper.GetCenterY();
            Point rightUpperPoint = new Point(x, y);
            points.Add(new Point(x, y));

            x = Canvas.GetLeft(rightLower) + rightLower.GetCenterX();
            y = Canvas.GetTop(rightLower) + rightLower.GetCenterY();
            Point rightLowerPoint = new Point(x, y);
            points.Add(new Point(x, y));

            x = Canvas.GetLeft(LeftLower) + LeftLower.GetCenterX();
            y = Canvas.GetTop(LeftLower) + LeftLower.GetCenterY();
            Point leftLowerPoint = new Point(x, y);
            points.Add(new Point(x, y));

            byte gray = 128;
            Brush strokeBrush = new SolidColorBrush(Color.FromArgb(128, gray, gray, gray));
            Polygon dendritesShape = new Polygon();
            dendritesShape.Points = points;
            dendritesShape.Fill = strokeBrush;
            dendritesShape.Stroke = Brushes.Black;
            dendritesShape.StrokeThickness = 2;

            canvas.Children.Add(dendritesShape);

        }

    }
}
