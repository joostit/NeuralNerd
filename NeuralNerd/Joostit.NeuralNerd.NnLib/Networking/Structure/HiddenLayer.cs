using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class HiddenLayer : List<HiddenNeuron>, ICalculatableNeuronLayer
    {
        public void Calculate()
        {
            foreach (CalculatedNeuron neuron in this)
            {
                neuron.CalculateActivation();
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
