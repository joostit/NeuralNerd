using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class InputLayer : INeuronLayer
    {

        public InputNeuron[] Neurons { get; private set; }

        public int Count
        {
            get
            {
                return Neurons.Length;
            }
        }

        Neuron INeuronLayer.this[int index]
        {
            get
            {
                return Neurons[index];
            }
        }

        public InputLayer(int size)
        {
            Neurons = new InputNeuron[size];
        }

        IEnumerator<Neuron> IEnumerable<Neuron>.GetEnumerator()
        {
            foreach (Neuron item in Neurons)
            {
                yield return item;
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Neuron item in Neurons)
            {
                yield return item;
            }
        }
    }
}
