using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class HiddenLayer : ICalculatableNeuronLayer
    {

        public HiddenNeuron[] Neurons { get; private set; }

        public int Count
        {
            get
            {
                return Neurons.Length;
            }
        }

        public HiddenLayer(int size)
        {
            Neurons = new HiddenNeuron[size];
        }

        public Neuron this[int index]
        {
            get
            {
                return Neurons[index];
            }
        }


        public void Calculate()
        {
            for(int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Calculate();
            }


            //Parallel.ForEach<CalculatedNeuron>(Neurons, (neuron) =>
            //{
            //    neuron.Calculate();
            //});
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Neuron item in Neurons)
            {
                yield return item;
            }
        }

        IEnumerator<Neuron> IEnumerable<Neuron>.GetEnumerator()
        {
            foreach (Neuron item in Neurons)
            {
                yield return item;
            }
        }
    }
}
