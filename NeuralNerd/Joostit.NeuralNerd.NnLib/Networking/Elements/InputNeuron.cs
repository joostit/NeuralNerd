using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking
{
    public class InputNeuron : Neuron
    {

        public new double Activation
        {
            get
            {
                return base.Activation;
            }
            set
            {
                base.Activation = value;
            }
        }

        public InputNeuron()
        {

        }

        public InputNeuron(string id)
            :base(id)
        {

        }

    }
}
