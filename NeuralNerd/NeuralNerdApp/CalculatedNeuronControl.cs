using Joostit.NeuralNerd.NnLib.Networking.Elements;
using NeuralNerdApp.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNerdApp
{
    class CalculatedNeuronControl : NeuronControl
    {

        public event EventHandler ConfigurationChanged;

        public new CalculatedNeuron Neuron
        {
            get
            {
                return base.Neuron as CalculatedNeuron;
            }
            set
            {
                base.Neuron = value;
            }
        }

        public CalculatedNeuronControl(CalculatedNeuron neuron)
            : base(neuron)
        {
            neuronIcon.MouseDown += NeuronIcon_MouseDown;
        }

        private void NeuronIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsEditable)
            {
                bool hasChanged = CalculatedNeuronEditWindow.Show(Neuron);

                if (hasChanged)
                {
                    Update();
                    RaiseConfigurationChanged();
                }
            }
        }

        private void RaiseConfigurationChanged()
        {
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        internal override void Clear()
        {
            base.Clear();

            ConfigurationChanged = null;
        }

    }
}
