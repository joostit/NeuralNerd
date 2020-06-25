using Joostit.NeuralNerd.NnLib.Learning;
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
    /// Interaction logic for NetworkPerformanceControl.xaml
    /// </summary>
    public partial class NetworkPerformanceControl : UserControl
    {

        public NetworkUiContext NetworkContext { get; private set; }

        public NetworkPerformanceControl()
        {
            InitializeComponent();
        }


        public void SetNetworkContext(NetworkUiContext context)
        {
            this.NetworkContext = context;
            SetOperationalState();
        }

        private void SetOperationalState()
        {
            if(NetworkContext.State == OperationalStates.Learning)
            {
                averageCostBox.IsEnabled = true;
                currentCostBox.IsEnabled = true;
                performanceGroup.IsEnabled = true;
            }
            else
            {
                averageCostBox.Text = "";
                averageCostBox.IsEnabled = false;
                currentCostBox.Text = "";
                currentCostBox.IsEnabled = false;
                performanceGroup.IsEnabled = false;
            }

            switch (NetworkContext.State)
            {
                case OperationalStates.Idle:
                    statusLabel.Content = "Idle";
                    break;
                case OperationalStates.LoadingStimuli:
                    statusLabel.Content = "Loading stimuli...";
                    break;
                case OperationalStates.Learning:
                    statusLabel.Content = "Learning...";
                    break;
                default:
                    throw new NotSupportedException($"Unsupported operational state: {NetworkContext.State}");
            }
        }

        public void UpdateLearningState(NetworkSnapshot snapshot)
        {
            SetOperationalState();

            if (snapshot?.Pass != null)
            {
                ShowImage(snapshot);
                UpdateValues(snapshot);
            }
            else
            {
                ClearImageAndValues();
            }
        }

        private void UpdateValues(NetworkSnapshot snapshot)
        {
            averageCostBox.Text = Math.Round(snapshot.AverageCostSoFar, 3).ToString();
            currentCostBox.Text = Math.Round(snapshot.Pass.Cost, 3).ToString();
        }

        private void ClearImageAndValues()
        {
            imageBox.Source = null;

            averageCostBox.Text = "";
            currentCostBox.Text = "";
        }

        private void ShowImage(NetworkSnapshot snapshot)
        {
            var image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
               snapshot.Pass.Stimulus.Image.Source.GetHbitmap(),
               IntPtr.Zero,
               System.Windows.Int32Rect.Empty,
               BitmapSizeOptions.FromWidthAndHeight(snapshot.Pass.Stimulus.Image.Source.Width, snapshot.Pass.Stimulus.Image.Source.Height));

            imageBox.Source = image;
        }
    }
}
