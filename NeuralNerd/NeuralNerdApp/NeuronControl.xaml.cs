using Joostit.NeuralNerd.NnLib.Networking.Elements;
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
    /// Interaction logic for NeuronControl.xaml
    /// </summary>
    public partial class NeuronControl : UserControl
    {

        public const double Size = 30;

        public Neuron Neuron { get; set; }

        public NeuronControl(Neuron neuron)
            :this()
        {
            this.Neuron = neuron;
        }

        private NeuronControl()
        {
            InitializeComponent();
        }


        public virtual void Update()
        {
            if (Neuron != null)
            {
                activationLabel.Text = Math.Round(Neuron.Activation, 2).ToString();
                SetBackColor();
            }
            contentCanvas.UpdateLayout();
        }


        internal virtual void Clear()
        {
            this.Neuron = null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
            UpdateLayout();
        }


        public virtual double GetCenterX()
        {
            return Size / 2;
        }

        public virtual double GetCenterY()
        {
            return Size / 2;
        } 


        private void SetBackColor()
        {
            byte v = (byte) Math.Round(Neuron.Activation * 255);
            Brush backBrush = new SolidColorBrush(Color.FromRgb(v, v, v));
            neuronIcon.Background = backBrush;

            if (v > 128)
            {
                activationLabel.Foreground = Brushes.Black;
            }
            else
            {
                activationLabel.Foreground = Brushes.LightGray;
            }
        }


    }
}
