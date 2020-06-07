using Joostit.NeuralNerd.NnLib.Networking.Elements;
using NeuralNerdApp.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNerdApp
{
    public class InputNeuronControl : NeuronControl
    {

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
        }

        private void NeuronIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double? result = NumericInputWindow.Show(0, 1, Neuron.Activation);

            if (result != null)
            {
                if (result.Value != Neuron.Activation)
                {
                    Neuron.Activation = result.Value;
                    Update();
                    RaiseActivationChanged();
                }
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

    }
}
