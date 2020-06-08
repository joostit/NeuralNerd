using Joostit.NeuralNerd.NnLib.Networking.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NeuralNerdApp
{
    class OutputNeuronControl : CalculatedNeuronControl
    {
        public const double OutputNeuronWidth = 200;

        private TextBlock nameLabel;

        public new OutputNeuron Neuron
        {
            get
            {
                return base.Neuron as OutputNeuron;
            }
            set
            {
                base.Neuron = value;
            }
        }

        public OutputNeuronControl(OutputNeuron neuron)
            : base(neuron)
        {
            this.Width = OutputNeuronWidth;
            CreateNameLabel();
            contentCanvas.Width = OutputNeuronWidth;
        }


        private void CreateNameLabel()
        {
            nameLabel = new TextBlock();
            nameLabel.FontSize = 11;
            nameLabel.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(nameLabel, Size + 5);
            Canvas.SetTop(nameLabel, (Size / 2) - 8);
            contentCanvas.Children.Add(nameLabel);
        }


        public override void Update()
        {
            base.Update();
            nameLabel.Text = Neuron.Name;
        }
    }
}
