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

        protected bool IsEditable { get; private set; }

        private double previousActivation = -1;

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
                if (Neuron.Activation != previousActivation)
                {
                    UpdateActivation(Neuron.Activation);
                }
            }
        }


        protected void UpdateActivation(double activation)
        {
            activationLabel.Text = Math.Round(activation, 2).ToString();
            SetBackColor(activation);
            previousActivation = activation;
        }


        internal virtual void Clear()
        {
            this.Neuron = null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        public virtual double GetCenterX()
        {
            return Size / 2;
        }

        public virtual double GetCenterY()
        {
            return Size / 2;
        } 


        private void SetBackColor(double activation)
        {
            byte v = (byte) Math.Round(activation * 255);
            Brush backBrush = GrayScaleBrushes.Get(v);
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

        private void neuronIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsEditable)
            {
                neuronIcon.BorderThickness = new Thickness(2);
            }
        }

        private void neuronIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            neuronIcon.BorderThickness = new Thickness(1);
        }

        internal void SetEditable(bool isEditable)
        {
            IsEditable = isEditable;
            UpdateActivation(Neuron.Activation);
        }

        public void SetLearningActivationState(double activation)
        {
            UpdateActivation(activation);
        }
    }
}
