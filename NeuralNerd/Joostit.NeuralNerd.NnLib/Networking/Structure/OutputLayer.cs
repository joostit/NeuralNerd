using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class OutputLayer : ICalculatableNeuronLayer
    {

        public OutputNeuron[] Neurons { get; private set; }

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


        public OutputLayer(int size)
        {
            Neurons = new OutputNeuron[size];
        }

        public void Calculate()
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Calculate();
            }

            //Parallel.ForEach<CalculatedNeuron>(this, (neuron) =>
            //{
            //    neuron.Calculate();
            //});
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
