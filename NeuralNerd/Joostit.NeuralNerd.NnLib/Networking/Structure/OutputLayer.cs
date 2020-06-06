using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking
{
    public class OutputLayer : List<OutputNeuron>, ICalculatableNeuronLayer
    {
        IEnumerator<Neuron> IEnumerable<Neuron>.GetEnumerator()
        {
            foreach (Neuron item in this)
            {
                yield return item;
            }
        }
    }
}
