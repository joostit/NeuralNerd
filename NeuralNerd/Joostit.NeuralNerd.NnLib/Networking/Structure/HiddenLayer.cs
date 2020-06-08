using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class HiddenLayer : List<HiddenNeuron>, ICalculatableNeuronLayer
    {
        Neuron INeuronLayer.this[int index]
        {
            get
            {
                return this[index];
            }
        }

        public void Calculate()
        {
            Parallel.ForEach<CalculatedNeuron>(this, (neuron) =>
            {
                neuron.Calculate();
            });
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
