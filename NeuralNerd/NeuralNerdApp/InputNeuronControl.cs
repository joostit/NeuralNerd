using Joostit.NeuralNerd.NnLib.Networking.Elements;
using NeuralNerdApp.Forms;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NeuralNerdApp
{
    public class InputNeuronControl : NeuronControl
    {
        private TextBlock nameLabel;

        public const double InputNeuronWidth = 200;

        public event EventHandler ActivationChanged;

        public new InputNeuron Neuron
        {
            get
            {
                return base.Neuron as InputNeuron;
            }
            set
            {
                base.Neuron = value;
            }
        }

        public InputNeuronControl(InputNeuron neuron)
            :base(neuron)
        {
            neuronIcon.MouseDown += NeuronIcon_MouseDown;
            neuronIcon.MouseWheel += NeuronIcon_MouseWheel;

            this.Width = InputNeuronWidth;

            Canvas.SetRight(neuronIcon, 0);

            CreateNameLabel();
            contentCanvas.Width = InputNeuronWidth;
        }

        private void NeuronIcon_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            double newActivation = Neuron.Activation - Math.Sign(e.Delta) / 10.0;
            e.Handled = true;

            newActivation = newActivation > 1 ? 1 : newActivation;
            newActivation = newActivation < 0 ? 0 : newActivation;

            Neuron.Activation = newActivation;

            RaiseActivationChanged();
        }

        private void CreateNameLabel()
        {
            nameLabel = new TextBlock();
            nameLabel.TextAlignment = TextAlignment.Right;
            nameLabel.FontSize = 11;
            nameLabel.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(nameLabel, 5);
            Canvas.SetTop(nameLabel, (Size / 2) - 8);

            nameLabel.Width = InputNeuronWidth - Size - 10;
            contentCanvas.Children.Add(nameLabel);
        }

        private void NeuronIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bool changed = InputNeuronEditWindow.Show(Neuron);

            if (changed)
            {
                Update();
                RaiseActivationChanged();
            }
        }

        private void RaiseActivationChanged()
        {
            ActivationChanged?.Invoke(this, EventArgs.Empty);
        }

        internal override void Clear()
        {
            base.Clear();

            ActivationChanged = null;
        }


        public override double GetCenterX()
        {
            return InputNeuronWidth - base.GetCenterX();
        }

        public override void Update()
        {
            base.Update();
            nameLabel.Text = Neuron.Name;
        }
    }
}
