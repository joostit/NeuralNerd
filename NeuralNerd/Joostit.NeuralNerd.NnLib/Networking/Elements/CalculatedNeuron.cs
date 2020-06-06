using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking
{
    public abstract class CalculatedNeuron : Neuron
    {

        public List<Dendrite> Dendrites { get; set; }

        public double Bias { get; set; }

    }
}
