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
            Update();
        }

        private NeuronControl()
        {
            InitializeComponent();
        }


        public void Update()
        {
            if (Neuron != null)
            {
                activationLabel.Text = Math.Round(Neuron.Activation, 2).ToString();
            }
        }


        internal virtual void Clear()
        {
            this.Neuron = null;
        }


    }
}
