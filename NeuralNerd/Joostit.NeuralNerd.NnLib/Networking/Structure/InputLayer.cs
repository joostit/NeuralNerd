using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class InputLayer : List<InputNeuron>, INeuronLayer
    {
        Neuron INeuronLayer.this[int index]
        {
            get
            {
                return this[index];
            }
        }

        IEnumerator<Neuron> IEnumerable<Neuron>.GetEnumerator()
        {
            foreach (Neuron item in this)
            {
                yield return item;
            }
        }
    }
}
